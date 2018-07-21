using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenHardwareMonitor.Hardware;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace TemperatureIconMeter
{
	public partial class FormMain : Form
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		extern static bool DestroyIcon(IntPtr handle);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int GetSystemMetrics(int nIndex);
		const int SM_CXSMICON = 49;

		public class Settings
		{
			[XmlElement(Type = typeof(XmlColor))] public Color SafeColor { get; set; } = Color.Green;
			[XmlElement(Type = typeof(XmlColor))] public Color WarningColor { get; set; } = Color.Yellow;
			[XmlElement(Type = typeof(XmlColor))] public Color DangerColor { get; set; } = Color.Red;
			public int WarningThreshold { get; set; } = 50;
			public int DangerThreshold { get; set; } = 60;
			public bool UseVerticalBars { get; set; } = false;
			public bool RunAtStartup { get; set; } = false;
			
			public List<string> SelectedSensorFullnames { get; set; } = new List<string>();

			public Settings()
			{
			}
		}

		private class SensorRecord
		{
			ISensor Sensor { get; set; }
			string FullName { get; set; }

			public SensorRecord(ISensor s)
			{
				this.Sensor = s;
				this.FullName = s.Hardware.Name + " " + s.Name;
			}
			public override string ToString()
			{
				return this.Sensor.Name;
			}
		}

		// private fields
		Settings settings = new Settings();
		readonly string settingsFilename = AppDomain.CurrentDomain.BaseDirectory + "settings.xml";
		readonly string maxArrow = "\u25b2";
		readonly string minArrow = "\u25bc";
		readonly List<ISensor> selectedSensors = new List<ISensor>();
		readonly List<ISensor> allSensors = new List<ISensor>();
		Computer myComputer = null;
		bool isClosing = false;
		bool allowShowForm = false;


		public FormMain()
		{
			InitializeComponent();

			InitTreeNodes();

			LoadSettings();

			UpdateAutoStartSetting();

			UpdateControlsFromSettings();

			UpdateSelectedSensorList();

			this.timerMain.Enabled = true;

			// set icon size in context menu strip with system icon size
			int iconSize = GetSystemMetrics(SM_CXSMICON);
			this.contextMenuStripMain.ImageScalingSize = new Size(iconSize, iconSize);
		}
		private void FormMain_Load(object sender, EventArgs e)
		{
			foreach (TreeNode node in this.treeViewSensors.Nodes)
			{
				node.HideCheckBox();
			}

			// set arrow symbols to up and down buttons
			this.buttonItemUp.Text = maxArrow;
			this.buttonItemDown.Text = minArrow;

			// update window client size to fix scaling problem in different OS versions
			this.ClientSize = new Size(
				this.ClientSize.Width,
				this.buttonCancel.Bottom + (this.ClientSize.Width - this.buttonCancel.Right)
				);

			// update anchor styles after updating window client size
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.labelSeparationBar3.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
		}
		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			// hide form when user close the form through UI
			if (e.CloseReason == CloseReason.UserClosing && isClosing == false)
			{
				e.Cancel = true;
				this.Hide();
			}
		}
		private void buttonItemUp_Click(object sender, EventArgs e)
		{
			MoveSelectedListBoxItem(-1);
		}
		private void buttonItemDown_Click(object sender, EventArgs e)
		{
			MoveSelectedListBoxItem(1);
		}
		private void buttonSafeColor_Click(object sender, EventArgs e)
		{
			ButtonColor_Click(sender, e);
		}
		private void buttonWarnColor_Click(object sender, EventArgs e)
		{
			ButtonColor_Click(sender, e);
		}
		private void buttonDangerColor_Click(object sender, EventArgs e)
		{
			ButtonColor_Click(sender, e);
		}
		private void buttonOK_Click(object sender, EventArgs e)
		{
			// update settings from controls' properties
			UpdateSettingsFromControls();

			// save settings to file
			SaveSettings();

			// update auto start system setting
			UpdateAutoStartSetting();

			// hide the form
			this.Hide();

			// update selected sensor list
			UpdateSelectedSensorList();
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			// hide the form
			this.Hide();
		}
		private void trackBarWarningThreshold_Scroll(object sender, EventArgs e)
		{
			settings.WarningThreshold = trackBarWarningThreshold.Value;
			if (settings.WarningThreshold >= settings.DangerThreshold)
				settings.DangerThreshold = settings.WarningThreshold + 1;

			UpdateLablesAndTrackBars();
		}
		private void trackBarDangerThreshold_Scroll(object sender, EventArgs e)
		{
			settings.DangerThreshold = trackBarDangerThreshold.Value;
			if (settings.DangerThreshold <= settings.WarningThreshold)
				settings.WarningThreshold = settings.DangerThreshold - 1;

			UpdateLablesAndTrackBars();
		}
		private void treeViewSensors_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (e.Action == TreeViewAction.Unknown) return;

			UpdateListBoxFromTreeView();
		}
		private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			// show this form
			allowShowForm = true; // this flag is used for override function SetVisibleCore()
			this.Visible = true;

			// update controls from current settings
			UpdateControlsFromSettings();
		}
		private void setupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// show this form
			allowShowForm = true; // this flag is used for override function SetVisibleCore()
			this.Visible = true;

			// update controls from current settings
			UpdateControlsFromSettings();
		}
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form f = new AboutBox();
			f.ShowDialog();
		}
		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// close this the application (thus close this main form)
			isClosing = true;
			Application.Exit();
		}
		private void timerMain_Tick(object sender, EventArgs e)
		{
			UpdateTreeView();

			UpdateIcon();

			UpdateNotifyIconTooltipText();
		}

		// private functions
		private int CountCheckedSensors()
		{
			int count = 0;
			foreach(TreeNode node in treeViewSensors.Nodes)
			{
				if (node.Checked) count++;

				foreach (TreeNode child in node.Nodes)
				{
					if (child.Checked) count++;
				}
			}

			return count;
		}
		private void SaveSettings()
		{
			// serialize current setting to xml file

			//try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Settings));

				using (StreamWriter writer = new StreamWriter(this.settingsFilename))
				{
					serializer.Serialize(writer, this.settings);
					writer.Close();
				}
			}
			//catch (Exception e) { Debug.Write(e.Message); }
		}
		private void LoadSettings()
		{
			// deserialize current setting from xml file

			//try
			{
				if (File.Exists(this.settingsFilename) == false) return;

				XmlSerializer serializer = new XmlSerializer(typeof(Settings));
				using (StreamReader reader = new StreamReader(this.settingsFilename))
				{
					this.settings = (Settings)serializer.Deserialize(reader);
					reader.Close();
				}
			}
			//catch (Exception e) { Debug.Write(e.Message); }
		}
		private void InitTreeNodes()
		{
			// find all hardwares and sensors with temperature data
			// also add them in tree view control

			// create Computer object
			myComputer = new Computer();
			myComputer.CPUEnabled = true;
			myComputer.MainboardEnabled = true;
			myComputer.HDDEnabled = true;
			myComputer.GPUEnabled = true;
			//myComputer.RAMEnabled = true;
			myComputer.Open();

			this.treeViewSensors.Nodes.Clear();

			foreach (var h in myComputer.Hardware)
			{
				if (h.Sensors.All(s => s.SensorType != SensorType.Temperature)) continue;

				TreeNode node = new TreeNode(h.Name);
				this.treeViewSensors.Nodes.Add(node);
				node.Tag = h;
				node.ForeColor = Color.Gray;
				node.HideCheckBox();

				var validSensors = h.Sensors.Where(s => s.SensorType == SensorType.Temperature).OrderBy(s => s.Name);
				foreach (var s in validSensors)
				{
					allSensors.Add(s);

					TreeNode childNode = new TreeNode(s.Name);
					childNode.Tag = s;
					node.Nodes.Add(childNode);
				}
			}
			this.treeViewSensors.ExpandAll();
		}
		private void UpdateTreeView()
		{
			foreach (TreeNode node in treeViewSensors.Nodes)
			{
				IHardware h = node.Tag as IHardware;
				h.Update();

				foreach (TreeNode child in node.Nodes)
				{
					ISensor s = child.Tag as ISensor;

					child.Text = $"{s.Name} [{s.Value}°C {maxArrow}{s.Max} {minArrow}{s.Min}]";
				}
			}
		}
		private void UpdateLablesAndTrackBars()
		{
			this.trackBarWarningThreshold.Value = settings.WarningThreshold;
			this.trackBarDangerThreshold.Value = settings.DangerThreshold;

			labelSafeTemperature.Text = "Safe Temperature: < " + settings.WarningThreshold + "°C";
			labelWarningTemperature.Text = "Warning Temperature: " + settings.WarningThreshold + " - " + settings.DangerThreshold + "°C";
			labelDangerTemperature.Text = "Danger Temperature: > " + settings.DangerThreshold + "°C";
		}
		private void UpdateControlsFromSettings()
		{
			// update control properties with the current settings
			this.buttonSafeColor.BackColor = settings.SafeColor;
			this.buttonWarnColor.BackColor = settings.WarningColor;
			this.buttonDangerColor.BackColor = settings.DangerColor;
			this.checkBoxUseVerticalBar.Checked = settings.UseVerticalBars;
			this.checkBoxRunAtStartup.Checked = settings.RunAtStartup;

			UpdateLablesAndTrackBars();

			// add all selected sensors in list box
			listBoxMain.Items.Clear();
			List<string> nonExistSensorNames = new List<string>();
			foreach (string name in settings.SelectedSensorFullnames)
			{
				bool exist = allSensors.Any(s => s.FullName() == name);
				if (exist)
				{
					string s = name;
					listBoxMain.Items.Add(s);
				}
				else
				{
					nonExistSensorNames.Add(name);
				}
			}

			// remove non-exist sensors from settings
			foreach (string name in nonExistSensorNames)
			{
				settings.SelectedSensorFullnames.Remove(name);
			}


			foreach (TreeNode node in treeViewSensors.Nodes)
			{
				foreach (TreeNode child in node.Nodes)
				{
					ISensor s = child.Tag as ISensor;

					if (listBoxMain.Items.Contains(s.FullName()))
						child.Checked = true;
				}
			}
		}
		private void UpdateSettingsFromControls()
		{
			settings.SafeColor = buttonSafeColor.BackColor;
			settings.WarningColor = buttonWarnColor.BackColor;
			settings.DangerColor = buttonDangerColor.BackColor;
			settings.UseVerticalBars = checkBoxUseVerticalBar.Checked;
			settings.RunAtStartup = checkBoxRunAtStartup.Checked;

			settings.WarningThreshold = trackBarWarningThreshold.Value;
			settings.DangerThreshold = trackBarDangerThreshold.Value;

			settings.SelectedSensorFullnames.Clear();
			foreach (var item in listBoxMain.Items)
			{
				settings.SelectedSensorFullnames.Add(item.ToString());
			}

		}
		private void UpdateListBoxFromTreeView()
		{
			foreach (TreeNode node in treeViewSensors.Nodes)
			{
				IHardware h = node.Tag as IHardware;

				foreach (TreeNode child in node.Nodes)
				{
					ISensor s = child.Tag as ISensor;

					string fullname = s.FullName();

					if (child.Checked && !this.listBoxMain.Items.Contains(fullname))
					{
						listBoxMain.Items.Add(fullname);
					}

					if (!child.Checked && listBoxMain.Items.Contains(fullname))
					{
						listBoxMain.Items.Remove(fullname);
					}
				}
			}
		}
		private void UpdateSelectedSensorList()
		{
			this.selectedSensors.Clear();
			foreach (string name in settings.SelectedSensorFullnames)
			{
				var sensor = this.allSensors.FirstOrDefault(s => s.FullName() == name);

				if (sensor != null) selectedSensors.Add(sensor);
			}
		}
		private void MoveSelectedListBoxItem(int direction)
		{
			// Checking selected item
			if (listBoxMain.SelectedItem == null || listBoxMain.SelectedIndex < 0)
				return; // No selected item - nothing to do

			// Calculate new index using move direction
			int newIndex = listBoxMain.SelectedIndex + direction;

			// Checking bounds of the range
			if (newIndex < 0 || newIndex >= listBoxMain.Items.Count)
				return; // Index out of range - nothing to do

			object selected = listBoxMain.SelectedItem;

			// Removing removable element
			listBoxMain.Items.Remove(selected);
			// Insert it in new position
			listBoxMain.Items.Insert(newIndex, selected);
			// Restore selection
			listBoxMain.SetSelected(newIndex, true);
		}
		private void ButtonColor_Click(object sender, EventArgs e)
		{
			// get the background color of the sender button
			Button b = sender as Button;
			this.colorDialogMain.Color = b.BackColor;

			// show color dialog to allow user to specify a new color
			DialogResult ret = colorDialogMain.ShowDialog(this);

			// update button background color if a new color is selected
			if (ret == DialogResult.OK)
			{
				b.BackColor = colorDialogMain.Color;
			}
		}
		private void UpdateAutoStartSetting()
		{
			if (settings.RunAtStartup)
			{
				AddAutoStartSchedule();
			}
			else
			{
				RemoveAutoStartSchedule();
			}
		}
		private void UpdateIcon()
		{
			// Update notify icon

			// create bitmap and corresponding graphics object
			Bitmap bitmapText = new Bitmap(16, 16);
			Graphics g = System.Drawing.Graphics.FromImage(bitmapText);

			// build some brushes and pens
			Brush safeBrush = new SolidBrush(settings.SafeColor);
			Brush warningBrush = new SolidBrush(settings.WarningColor);
			Brush dangerBrush = new SolidBrush(settings.DangerColor);
			Pen shadowPen = new Pen(Color.FromArgb(128, Color.Black));

			// clear background and draw bounding box
			g.Clear(Color.Transparent);
			Pen pen = new Pen(Color.DarkGray);
			g.DrawLine(pen, 0, 0, 0, 15);
			g.DrawLine(pen, 0, 15, 15, 15);
			g.DrawLine(pen, 15, 15, 15, 0);
			g.DrawLine(pen, 15, 0, 0, 0);

			// compute bar height
			if (this.selectedSensors.Count > 0)
			{
				int nReadings = this.selectedSensors.Count;
				float barHeight = 16 / nReadings;

				// render all bars
				if (settings.UseVerticalBars)
				{
					float left = 0;
					for (int i = 0; i < nReadings; i++)
					{
						float reading = selectedSensors[i].Value ?? 0;
						float h = (reading * 16 / 100);
						Brush br = reading < settings.WarningThreshold ? safeBrush :
							reading < settings.DangerThreshold ? warningBrush : dangerBrush;
						g.FillRectangle(br, left, 16 - h, barHeight, h);
						left += barHeight;
						g.DrawLine(shadowPen, left - 1, 16 - h + 0.5f, left - 1, 16);
					}
				}
				else // use horizontal bars
				{
					float top = 0;
					for (int i = 0; i < nReadings; i++)
					{
						float reading = selectedSensors[i].Value ?? 0;
						float h = (reading * 16 / 100);
						Brush br = reading < settings.WarningThreshold ? safeBrush :
							reading < settings.DangerThreshold ? warningBrush : dangerBrush;
						g.FillRectangle(br, 0, top, h, barHeight);
						top += barHeight;
						g.DrawLine(shadowPen, 0, top - 1, h - 0.5f, top - 1);
					}
				}
			}

			// create icon from bitmap
			IntPtr hIcon = (bitmapText.GetHicon());
			Icon newIcon = Icon.FromHandle(hIcon);
			Icon oldIcon = notifyIconMain.Icon;
			notifyIconMain.Icon = newIcon;
			if (oldIcon != null) DestroyIcon(oldIcon.Handle); // remember to delete the old icon

			// remember to dispose objects
			safeBrush.Dispose();
			warningBrush.Dispose();
			dangerBrush.Dispose();
			shadowPen.Dispose();
		}
		private void UpdateNotifyIconTooltipText()
		{
			// update notify icon's tooltip text

			StringBuilder sb = new StringBuilder();
			foreach (ISensor s in selectedSensors)
			{
				sb.AppendLine($"{s.Name} {s.Value}°C {maxArrow}{s.Max} {minArrow}{s.Min}");
			}

			// make sure the tooltip text has at most 64 characters
			if (sb.Length >= 127) sb.Remove(127, sb.Length - 127);

			// update the text value
			//notifyIconMain.Text = sb.ToString();
			SetNotifyIconText(notifyIconMain, sb.ToString());
		}
		private void SetNotifyIconText(NotifyIcon ni, string text)
		{
			if (text.Length >= 128) throw new ArgumentOutOfRangeException("Text limited to 127 characters");
			Type t = typeof(NotifyIcon);
			BindingFlags hidden = BindingFlags.NonPublic | BindingFlags.Instance;
			t.GetField("text", hidden).SetValue(ni, text);
			if ((bool)t.GetField("added", hidden).GetValue(ni))
				t.GetMethod("UpdateIcon", hidden).Invoke(ni, new object[] { true });
		}

		private void AddAutoStartSchedule()
		{
			// Get the service on local machine
			using (TaskService ts = new TaskService())
			{
				// Create a new task definition and assign properties
				TaskDefinition td = ts.NewTask();
				td.RegistrationInfo.Description = "Temperature Icon Meter";
				td.Principal.RunLevel = TaskRunLevel.Highest; 

				// Create a trigger 
				td.Triggers.Add(new LogonTrigger());

				// Create an action
				string path = Application.ExecutablePath;
				string workingDirectory = Path.GetDirectoryName(path);
				td.Actions.Add(new ExecAction(path, null, workingDirectory));

				// Register the task in the root folder
				ts.RootFolder.RegisterTaskDefinition("TemperatureIconMeter", td);
			}
		}
		private void RemoveAutoStartSchedule()
		{
			using (TaskService ts = new TaskService())
			{
				ts.RootFolder.DeleteTask("TemperatureIconMeter", exceptionOnNotExists: false);
			}
		}

		// override function for disable show window on startup
		protected override void SetVisibleCore(bool value)
		{
			base.SetVisibleCore(allowShowForm ? value : false);
		}

	}

	public static class ISensorExtensions
	{
		public static string FullName(this ISensor sensor)
		{
			return sensor.Hardware.Name + " " + sensor.Name;
		}
	}
}
