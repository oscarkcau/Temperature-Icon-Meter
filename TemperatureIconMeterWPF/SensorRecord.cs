using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureIconMeterWPF
{
	[Serializable]
	public class SensorRecord
	{
		// public fields
		public string HardwareName { get; set; }
		public string SensorName { get; set; }
		public bool IsSelected { get; set; }
		public string DisplayName { get; set; }

		// constructors
		public SensorRecord() { }

		public SensorRecord(string hardwareName, string sensorName, bool isSelected, string displayName)
		{
			HardwareName = hardwareName;
			SensorName = sensorName;
			IsSelected = isSelected;
			DisplayName = displayName;
		}
	}
}
