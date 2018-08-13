using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace TemperatureIconMeterWPF
{
	[Serializable]
	public class Settings
	{
		public Color SafeColor { get; set; } = Colors.Green;
		public Color WarningColor { get; set; } = Colors.Yellow;
		public Color DangerColor { get; set; } = Colors.Red;
		public int WarningThreshold { get; set; } = 50;
		public int DangerThreshold { get; set; } = 60;
		public bool UseVerticalBars { get; set; } = false;
		public bool RunAtStartup { get; set; } = false;

		[XmlIgnore]
		public Dictionary<(string, string), (bool, string)> SensorRecords { get; set; } = new Dictionary<(string, string), (bool, string)>();
		public List<(string, string, bool, string)> SensorRecordsList { get; set; } = new List<(string, string, bool, string)>();

		// public methods
		public Settings Clone()
		{
			// clone a new Settings instance

			SensorRecordsToList();

			//serualize to memory stream
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));
			MemoryStream memStream = new MemoryStream();
			serializer.Serialize(memStream, this);

			// descerialize to new instance and return
			memStream.Position = 0;
			Settings cloned =  ((Settings)serializer.Deserialize(memStream));

			cloned.SensorRecordsFromList();

			return cloned;
		}
		public void SaveToFile(string filename)
		{
			// serialize settings to xml file


			SensorRecordsToList();

			// serialize to file stream
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));
			using (StreamWriter writer = new StreamWriter(filename))
			{
				serializer.Serialize(writer, this);
				writer.Close();
			}
		}
		public static Settings LoadFromFile(string filename)
		{
			// read and deserialize settings from file

			// ensure file exist before reading
			if (File.Exists(filename) == false) throw new FileNotFoundException();

			// read and deserialize from file stream
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));
			using (StreamReader reader = new StreamReader(filename))
			{
				Settings s = (Settings)serializer.Deserialize(reader);
				reader.Close();

				s.SensorRecordsFromList();

				return s;
			}
		}

		// private methods
		private void SensorRecordsToList()
		{
			SensorRecordsList = new List<(string, string, bool, string)>();

			if (SensorRecords != null)
				foreach (var key in SensorRecords.Keys)
				{
					var value = SensorRecords[key];
					SensorRecordsList.Add((key.Item1, key.Item2, value.Item1, value.Item2));
				}
		}
		private void SensorRecordsFromList()
		{
			this.SensorRecords = new Dictionary<(string, string), (bool, string)>();

			if (SensorRecordsList != null)
				foreach (var rec in SensorRecordsList)
				{
					var key = (rec.Item1, rec.Item2);
					var value = (rec.Item3, rec.Item4);
					SensorRecords.Add(key, value);
				}
		}
	}
}
