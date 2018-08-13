﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			window.ShowDialog();
		}
	}
}