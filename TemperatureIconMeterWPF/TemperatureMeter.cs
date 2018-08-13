using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using OpenHardwareMonitor.Hardware;

namespace TemperatureIconMeterWPF
{
	class TemperatureMeter : INotifyPropertyChanged
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		extern static bool DestroyIcon(IntPtr handle);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int GetSystemMetrics(int nIndex);
		const int SM_CXSMICON = 49;

		// readonly private fields
		readonly int systemTrayIconSize;
		readonly string maxArrow = "\u25b2";
		readonly string minArrow = "\u25bc";

		// private fields
		Settings settings;
		Computer myComputer = null;
		DispatcherTimer timer = new DispatcherTimer();
		Icon _defaultTrayIcon, _mainTrayIcon;
		string _tooltip;

		// public property
		public Settings Settings { get => settings; set { settings = value; SetSensorRecords(settings.SensorRecords); } }
		public ObservableCollection<HardwareTreeNode> HardwareTreeNodes { get; } = new ObservableCollection<HardwareTreeNode>();
		public Icon DefaultTrayIcon {
			get => _defaultTrayIcon;
			set => SetField(ref _defaultTrayIcon, value);
		}
		public Icon MainTrayIcon {
			get => _mainTrayIcon;
			set => SetField(ref _mainTrayIcon, value);
		}
		public string Tooltip {
			get => _tooltip;
			set => SetField(ref _tooltip, value);
		}

		// constructor
		public TemperatureMeter(Settings settings)
		{
			// get the default tray icon size
			systemTrayIconSize = GetSystemMetrics(SM_CXSMICON);

			// initialize the temperature sensor nodes
			InitSensorNodes();

			Settings = settings;

			// initialize timer and start measurement
			timer.Tick += new EventHandler(Timer_Tick);
			timer.Interval = new TimeSpan(0, 0, 1);
			timer.Start();
		}

		// public methods
		public Dictionary<(string, string), (bool, string)> GetSensorRecords()
		{
			var records = new Dictionary<(string, string), (bool, string)>();

			// loop over all sensors
			foreach (var h in HardwareTreeNodes)
			{
				string hardwareName = h.Name;

				foreach (var s in h.Sensors.Where(s => s.IsSelected))
				{
					// store all selected sensors
					string sensorName = s.Name;
					records.Add((hardwareName, sensorName), (s.IsSelected, s.DisplayName));
				}
			}

			return records;
		}
		public void SetSensorRecords(Dictionary<(string, string), (bool isSelected, string displayName)> records)
		{
			// loop over all sensors
			foreach (var h in HardwareTreeNodes)
			{
				string hardwareName = h.Name;

				foreach (var s in h.Sensors)
				{
					string sensorName = s.Name;

					var key = (hardwareName, sensorName);

					if (records.ContainsKey(key))
					{
						// set sensor tree node propertiew if records contains the sensor key
						s.IsSelected = records[key].isSelected;
						s.DisplayName = records[key].displayName;
					}
				}
			}
		}
		public void ResetMinMaxReadings()
		{
			foreach (var h in HardwareTreeNodes)
			{
				foreach (var s in h.Sensors)
					s.ResetMinMaxReadings();
			}
		}

		// event handler
		private void Timer_Tick(object sender, EventArgs e)
		{
			// first get new readings from performance counters
			UpdateReadings();

			// update icon image and tooltip of main tray icon
			if (MainTrayIcon != null && MainTrayIcon != DefaultTrayIcon)
				DestroyIcon(MainTrayIcon.Handle);
			MainTrayIcon = BuildTemperatureMeterIcon();

			Tooltip = BuildLogicalProcessorTooltip();
		}

		// private methods
		void InitSensorNodes()
		{
			// create Computer object
			myComputer = new Computer
			{
				CPUEnabled = true,
				MainboardEnabled = true,
				HDDEnabled = true,
				GPUEnabled = true,
				RAMEnabled = true
			};
			myComputer.Open();

			// loop over all hardware objects
			foreach (var h in myComputer.Hardware)
			{
				// skip hardware if it has NO temperature sensor
				if (h.Sensors.All(s => s.SensorType != SensorType.Temperature)) continue;

				// create hardware tree node
				var hNode = new HardwareTreeNode(h);
				HardwareTreeNodes.Add(hNode);

				// get collection of all temperature sensors of current hardware
				var TemperatureSensors = h.Sensors
					.Where(s => s.SensorType == SensorType.Temperature)
					.OrderBy(s => s.Name);

				// loop over all temperature sensors
				foreach (var s in TemperatureSensors)
				{
					// create sensor tree node
					var sNode = new SensorTreeNode(s, hNode);
					hNode.Sensors.Add(sNode);
				}
			}
		}
		void UpdateReadings()
		{
			foreach (var h in HardwareTreeNodes)
			{
				h.Update();

				foreach (var s in h.Sensors)
				{
					s.Update();
				}
			}
		}
		Icon BuildTemperatureMeterIcon()
		{
			// create brushes for drawing
			Brush safeBrush = new SolidBrush(settings.SafeColor.ToGDIColor());
			Brush warningBrush = new SolidBrush(settings.WarningColor.ToGDIColor());
			Brush dangerBrush = new SolidBrush(settings.DangerColor.ToGDIColor());

			// create list
			List<(float, Brush)> list = new List<(float, Brush)>();

			// loop over all sensors
			foreach (var h in HardwareTreeNodes)
			{
				string hardwareName = h.Name;
				
				foreach (var s in h.Sensors)
				{
					string sensorName = s.Name;

					// add current value and corrsponding brush if it is selected in setting
					if (settings.SensorRecords.ContainsKey((hardwareName, sensorName)))
					{
						list.Add(
							(s.Value,
							s.Value < settings.WarningThreshold ? safeBrush :
							s.Value < settings.DangerThreshold ? warningBrush :
							dangerBrush)
							);
					}
				}
			}

			Icon icon = BuildIcon(list, true);

			// release resource used by brushes
			safeBrush.Dispose();
			warningBrush.Dispose();
			dangerBrush.Dispose();

			return icon;
		}
		Icon BuildIcon(IEnumerable<(float, Brush)> list, bool drawShadow = false)
		{
			// draw new icon according the input reading values and brushes

			// return default icon if no reading to display
			int nReadings = list.Count();
			if (nReadings == 0) return _defaultTrayIcon;

			// determine icon size
			int iconSize =
				nReadings <= 4 ? 16 :
				32 < systemTrayIconSize ? 32 :
				systemTrayIconSize;

			// create bitmap and corresponding graphics object
			Bitmap bmp = new Bitmap(iconSize, iconSize);
			Graphics g = System.Drawing.Graphics.FromImage(bmp);
			Pen shadowPen = new Pen(Color.FromArgb(128, Color.Black));

			// clear background and draw bounding box
			g.Clear(Color.Transparent);
			Pen pen = new Pen(Color.DarkGray);
			int t = iconSize - 1;
			g.DrawLine(pen, 0, 0, 0, t);
			g.DrawLine(pen, 0, t, t, t);
			g.DrawLine(pen, t, t, t, 0);
			g.DrawLine(pen, t, 0, 0, 0);

			// compute bar height
			float barHeight = iconSize / nReadings;

			// render all bars
			if (settings.UseVerticalBars)
			{
				float left = 0;
				foreach (var (value, brush) in list)
				{
					float height = value * iconSize / 100.0f;
					g.FillRectangle(brush, left, iconSize - height, barHeight, height);
					left += barHeight;
					if (drawShadow)
						g.DrawLine(shadowPen, left - 1, iconSize - height + 0.5f, left - 1, iconSize);
				}
			}
			else // use horizontal bars
			{
				float top = 0;
				foreach (var (value, brush) in list)
				{
					float height = value * iconSize / 100.0f;
					g.FillRectangle(brush, 0, top, height, barHeight);
					top += barHeight;
					if (drawShadow)
						g.DrawLine(shadowPen, 0, top - 1, height - 0.5f, top - 1);
				}
			}

			// remember to dispose objects
			shadowPen.Dispose();

			return System.Drawing.Icon.FromHandle(bmp.GetHicon());
		}
		string BuildLogicalProcessorTooltip()
		{
			// build notify icon's tooltip text


			// build the text
			StringBuilder sb = new StringBuilder();

			// loop over all sensors
			foreach (var h in HardwareTreeNodes)
			{
				string hardwareName = h.Name;

				foreach (var s in h.Sensors)
				{
					string sensorName = s.Name;

					// add current value and corrsponding brush if it is selected in setting
					if (settings.SensorRecords.ContainsKey((hardwareName, sensorName)))
					{
						sb.AppendLine($"{s.DisplayName} {s.Value}°C {maxArrow}{s.Max} {minArrow}{s.Min}");
					}
				}
			}

			// make sure the tooltip text has at most 128 characters
			//
			// do not need this checking anymore
			// when using Hardcodet.Wpf.TaskbarNotification.TaskbarIcon
			//if (sb.Length >= 128) sb.Remove(127, sb.Length - 127);

			// return the text value
			return sb.ToString().TrimEnd();
		}

		// INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return;
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	class HardwareTreeNode
	{
		// private field
		IHardware hardware;

		// public properties
		public string Name { get => hardware.Name; }
		public ObservableCollection<SensorTreeNode> Sensors { get; private set; }

		// constructor
		public HardwareTreeNode(IHardware hardware)
		{
			this.hardware = hardware;
			this.Sensors = new ObservableCollection<SensorTreeNode>();
		}

		// publid method
		public void Update() { hardware.Update(); }
	}

	class SensorTreeNode : INotifyPropertyChanged
	{
		// private field
		ISensor sensor;
		HardwareTreeNode _parent;
		float _min, _max, _value;
		bool _isSelected = false;
		string _displayName;

		// public properties
		public string Name { get => sensor.Name; }
		public HardwareTreeNode Parent { get => _parent; }
		public float Min { get => _min; private set => SetField(ref _min, value); }
		public float Max { get => _max; private set => SetField(ref _max, value); }
		public float Value { get => _value; private set => SetField(ref _value, value); }
		public bool IsSelected { get => _isSelected; set => SetField(ref _isSelected, value); }
		public string DisplayName { get => _displayName; set => SetField(ref _displayName, value); }

		// constructor
		public SensorTreeNode(ISensor sensor, HardwareTreeNode parent)
		{
			this.sensor = sensor;
			this.DisplayName = sensor.Name;

			this._parent = parent;
		}

		// public method
		public void Update()
		{
			if (sensor.Min.HasValue) Min = (float)sensor.Min;
			if (sensor.Max.HasValue) Max = (float)sensor.Max;
			if (sensor.Value.HasValue) Value = (float)sensor.Value;
		}
		public void ResetMinMaxReadings()
		{
			sensor.ResetMin();
			sensor.ResetMax();
		}

		// INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return;
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public static class ConvertExtesions
	{
		public static System.Drawing.Color ToGDIColor(this System.Windows.Media.Color c)
		{
			return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
		}

	}
}
