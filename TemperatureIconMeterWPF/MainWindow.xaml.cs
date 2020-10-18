using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TemperatureIconMeterWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// constructor
		public MainWindow()
		{
			InitializeComponent();

			this.Visibility = Visibility.Hidden;

			var vm = this.DataContext as MainViewModel;

			// setup property changed listener for tray icon update
			vm.TemperatureMeter.PropertyChanged += TemperatureMeter_PropertyChanged;

			// setup defailt icon for TemperatureMeter object
			var uri = new Uri(@"pack://application:,,,/icon.ico", UriKind.RelativeOrAbsolute);
			Stream iconStream = Application.GetResourceStream(uri).Stream;
			vm.TemperatureMeter.DefaultTrayIcon = new System.Drawing.Icon(iconStream);

			// initial ComboBox for language selection
			ResourceSet resourceSet =
				Properties.LanguageResource.ResourceManager.GetResourceSet(
					CultureInfo.CurrentUICulture, true, true
					);
			cmbLanguage.ItemsSource = resourceSet;
		}

		// event handlers
		private void TemperatureMeter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			// update tray icons if needed
			var vm = this.DataContext as MainViewModel;
			if (e.PropertyName == "MainTrayIcon") MainTaskbarIcon.Icon = vm.TemperatureMeter.MainTrayIcon;
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// cancel window closing and hide the window
			e.Cancel = true;
			this.Hide();

			// disable any modify setting fields by calling CancelSettingsUpdate command
			var vm = this.DataContext as MainViewModel;
			vm.CancelSettingsUpdate.Execute(null);
		}
		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			// restart application if languate setting is updated
			if (Properties.Settings.Default.Language != System.Threading.Thread.CurrentThread.CurrentUICulture.Name)
			{
				// set the is restarting flag
				Properties.Settings.Default.IsRestarting = true;
				Properties.Settings.Default.Save();

				// start a new instance of application
				System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);

				// close the current instance of application
				Application.Current.Shutdown();
			}

			this.Hide();
		}
		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Hide();
		}
		private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
		{
			this.Show();
		}
		private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
		{
			AboutBox aboutBox = new AboutBox();
			aboutBox.ShowDialog();
		}
		private void MenuItemClose_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}
		private void SensorTreeNodeHyperlink_Click(object sender, RoutedEventArgs e)
		{
			Hyperlink link = sender as Hyperlink;
			SensorTreeNode node = link.DataContext as SensorTreeNode;
			WindowDisplayNameEditor window = new WindowDisplayNameEditor(node);
			window.Owner = this;
			window.ShowDialog();
		}
	}

	public class DrawingColorToWindowsMediaColor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			System.Drawing.Color c = (System.Drawing.Color)value;
			return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			System.Windows.Media.Color c = (System.Windows.Media.Color)value;
			return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
		}
	}
}