using System;
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
		private bool isAnchorMouseDown = false;
		private Point mouseDownPosition;
		private double maxReadingTextWidth = 0;
		private double minReadingTextWidth = 0;
		private double currentReadingTextWidth = 0;

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
			if (sender == ImageClose)
			{
				// hide popup window
				Popup p = Parent as Popup;
				p.IsOpen = false;
				p.StaysOpen = false;
			}

			if (sender == ImageConfig)
			{
				// hide popup window
				Popup p = Parent as Popup;
				p.IsOpen = false;
				p.StaysOpen = false;

				// show setting window
				MainViewModel vm = DataContext as MainViewModel;
				vm.MainWindow.Show();
			}

			if (sender == ImageAbout)
			{
				// hide popup window
				Popup p = Parent as Popup;
				p.IsOpen = false;

				// show about dialog
				AboutBox aboutBox = new AboutBox();
				aboutBox.ShowDialog();
			}

			if (sender == ImageResetMaxMin)
			{
				MainViewModel vm = DataContext as MainViewModel;
				vm.ResetMinMaxReadings.Execute(null);
			}
		}
		private void ImageAnchor_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// set flag
			isAnchorMouseDown = true;

			// record mouse down position for calculate relative movement
			mouseDownPosition = e.GetPosition(this);

			// force button to receive mouse events
			ImageAnchor.CaptureMouse();
		}
		private void ImageAnchor_MouseMove(object sender, MouseEventArgs e)
		{
			// if it is mouse drag
			if (isAnchorMouseDown)
			{
				// calcute relative mouse movement
				Point pos = e.GetPosition(this);
				double dx = pos.X - mouseDownPosition.X;
				double dy = pos.Y - mouseDownPosition.Y;

				// update popup window position
				Popup p = this.Parent as Popup;
				p.HorizontalOffset += dx;
				p.VerticalOffset += dy;
			}
		}
		private void ImageAnchor_MouseUp(object sender, MouseButtonEventArgs e)
		{
			// reset flag
			isAnchorMouseDown = false;

			// release mouse capture 
			ImageAnchor.ReleaseMouseCapture();

			// keep popup be shown even focus is lost
			Popup p = Parent as Popup;
			p.StaysOpen = true;
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
