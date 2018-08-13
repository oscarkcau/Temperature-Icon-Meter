using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TemperatureIconMeterWPF
{
	/// <summary>
	/// Interaction logic for WindowDisplayNameEditor.xaml
	/// </summary>
	public partial class WindowDisplayNameEditor : Window
	{
		string originalDisplayName;

		internal WindowDisplayNameEditor(SensorTreeNode node)
		{
			InitializeComponent();

			this.DataContext = node;
			originalDisplayName = node.DisplayName;
		}

		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			SensorTreeNode node = this.DataContext as SensorTreeNode;
			node.DisplayName = originalDisplayName;

			this.DialogResult = false;
			this.Close();
		}

		private void ButtonReset_Click(object sender, RoutedEventArgs e)
		{
			SensorTreeNode node = this.DataContext as SensorTreeNode;
			node.DisplayName = node.Name;
		}
	}
}
