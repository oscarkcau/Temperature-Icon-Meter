using System;
using System.Collections.Generic;
using System.Linq;
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
	public partial class PopupWindow : UserControl
	{
		// private fields
		double minReadingTextWidth = 0;
		double maxReadingTextWidth = 0;
		double currentReadingTextWidth = 0;

		// constructir
		public PopupWindow()
		{
			InitializeComponent();
		}

		// event handlers
		private void ImageClose_MouseUp(object sender, MouseButtonEventArgs e)
		{
			var p = this.Parent as Popup;
			p.IsOpen = false;
		}
		private void ImageClose_TouchUp(object sender, TouchEventArgs e)
		{
			var p = this.Parent as Popup;
			p.IsOpen = false;
		}
		private void MaxTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.WidthChanged)
			{
				if (e.NewSize.Width > maxReadingTextWidth)
				{
					maxReadingTextWidth = e.NewSize.Width;
				}
				else
				{
					((TextBlock)sender).Width = maxReadingTextWidth;
				}
			}
		}
		private void MinTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.WidthChanged)
			{
				if (e.NewSize.Width > minReadingTextWidth)
				{
					minReadingTextWidth = e.NewSize.Width;
				}
				else
				{
					((TextBlock)sender).Width = minReadingTextWidth;
				}
			}
		}
		private void CurrentTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.WidthChanged)
			{
				if (e.NewSize.Width > currentReadingTextWidth)
				{
					currentReadingTextWidth = e.NewSize.Width;
				}
				else
				{
					((TextBlock)sender).Width = currentReadingTextWidth;
				}
			}
		}
	}
}
