﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
	/// Interaction logic for PopupWindow.xaml
	/// </summary>
	public partial class PopupWindow : UserControl, INotifyPropertyChanged
	{
		// private fields
		// private fields
		bool pinned = false;
		bool isAnchorMouseDown = false;
		Point mouseDownPosition;
		double maxReadingTextWidth = 0;
		double minReadingTextWidth = 0;
		double currentReadingTextWidth = 0;

		// public properties
		public double MaxReadingTextWidth {
			get => maxReadingTextWidth;
			set => SetField(ref maxReadingTextWidth, value);
		}
		public double MinReadingTextWidth {
			get => minReadingTextWidth;
			set => SetField(ref minReadingTextWidth, value);
		}
		public double CurrentReadingTextWidth {
			get => currentReadingTextWidth;
			set => SetField(ref currentReadingTextWidth, value);
		}

		// constructir
		public PopupWindow()
		{
			InitializeComponent();
		}

		// event handlers
		private void MaxTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.WidthChanged)
			{
				if (e.NewSize.Width > MaxReadingTextWidth)
				{
					MaxReadingTextWidth = e.NewSize.Width;
				}
			}
		}
		private void MinTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.WidthChanged)
			{
				if (e.NewSize.Width > MinReadingTextWidth)
				{
					MinReadingTextWidth = e.NewSize.Width;
				}
			}
		}
		private void CurrentTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.WidthChanged)
			{
				if (e.NewSize.Width > CurrentReadingTextWidth)
				{
					CurrentReadingTextWidth = e.NewSize.Width;
				}
			}
		}
		private void Image_MouseUp(object sender, MouseButtonEventArgs e)
		{
			var img = sender as Image;
			ImagePressed(img);
		}
		private void Image_TouchUp(object sender, TouchEventArgs e)
		{
			var img = sender as Image;
			ImagePressed(img);
		}
		private void ImagePressed(Image sender)
		{
			if (sender == this.ImagePin)
			{
				if (pinned == false)
				{
					pinned = true;
					ImagePin.RenderTransform = new TranslateTransform(-2, 4);
					ImagePin.Clip = new RectangleGeometry(new Rect(0, 0, 16, 12));
					var p = this.Parent as Popup;
					p.StaysOpen = true;
				}
				else
				{
					pinned = false;
					ImagePin.RenderTransform = Transform.Identity;
					ImagePin.Clip = null;
					var p = this.Parent as Popup;
					p.StaysOpen = false;
					p.IsOpen = false;
				}
			}
			if (sender == this.ImageClose)
			{
				// hide popup window
				var p = this.Parent as Popup;
				p.IsOpen = false;
			}

			if (sender == this.ImageConfig)
			{
				// hide popup window
				var p = this.Parent as Popup;
				p.IsOpen = false;

				// show setting window
				var vm = this.DataContext as MainViewModel;
				vm.MainWindow.Show();
			}

			if (sender == this.ImageAbout)
			{
				// hide popup window
				var p = this.Parent as Popup;
				p.IsOpen = false;

				// show about dialog
				AboutBox aboutBox = new AboutBox();
				aboutBox.ShowDialog();
			}

			if (sender == this.ImageResetMaxMin)
			{
				var vm = this.DataContext as MainViewModel;
				vm.ResetMinMaxReadings.Execute(null);
			}
		}
		private void ImageAnchor_MouseDown(object sender, MouseButtonEventArgs e)
		{
			isAnchorMouseDown = true;
			mouseDownPosition = e.GetPosition(this);
			ImageAnchor.CaptureMouse();
		}
		private void ImageAnchor_MouseMove(object sender, MouseEventArgs e)
		{
			if (isAnchorMouseDown)
			{
				var pos = e.GetPosition(this);
				var dx = pos.X - mouseDownPosition.X;
				var dy = pos.Y - mouseDownPosition.Y;
				var p = this.Parent as Popup;
				p.HorizontalOffset += dx;
				p.VerticalOffset += dy;
			}
		}
		private void ImageAnchor_MouseUp(object sender, MouseButtonEventArgs e)
		{
			isAnchorMouseDown = false;
			ImageAnchor.ReleaseMouseCapture();
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
}
