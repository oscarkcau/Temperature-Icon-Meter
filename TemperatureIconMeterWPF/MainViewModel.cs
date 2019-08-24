using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32.TaskScheduler;

namespace TemperatureIconMeterWPF
{
    class MainViewModel : INotifyPropertyChanged
	{
		// private fields
		TemperatureMeter _temperatureMeter;

		// properties
		public Properties.Settings Settings { get; } = Properties.Settings.Default;
		public TemperatureMeter TemperatureMeter { get => _temperatureMeter; private set => SetField(ref _temperatureMeter, value); }
		public int MinTemperature { get => 0; }
		public int MaxTemperature { get => 100; }
		public int WarningThreshold {
			get => Settings.WarningThreshold;
			set
			{
				if (value < MinTemperature) value = MinTemperature;
				if (value > MaxTemperature) value = MaxTemperature;
				Settings.WarningThreshold = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WarningThreshold"));
				if (value >= DangerThreshold) DangerThreshold = value + 1;
			}
		}
		public int DangerThreshold {
			get => Settings.DangerThreshold;
			set
			{
				if (value < MinTemperature) value = MinTemperature;
				if (value > MaxTemperature) value = MaxTemperature;
				Settings.DangerThreshold = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DangerThreshold"));
				if (value <= WarningThreshold) WarningThreshold = value - 1;
			}
		}
		public ICommand SaveAndReset { get; private set; }
		public ICommand LoadAndReset { get; private set; }
		public ICommand CancelSettingsUpdate { get; private set; }
		public ICommand RescanSystem { get; private set; }
		public ICommand ResetMinMaxReadings { get; private set; }

		// constructors
		public MainViewModel()
		{
			// initial all public ICommand objects
			InitCommands();

			// load settings from file and reset meter
			_LoadAndReset();
		}

		// private methods
		void InitCommands()
		{
			//
			// init ICommand objects for binding
			//

			SaveAndReset = new RelayCommand(_SaveAndReset);
			LoadAndReset = new RelayCommand(_LoadAndReset);
			CancelSettingsUpdate = new RelayCommand(_CancelSettingsUpdate);
			RescanSystem = new RelayCommand(_RescanSystem);
			ResetMinMaxReadings = new RelayCommand(_ => TemperatureMeter.ResetMinMaxReadings());
		}
		void _SaveAndReset(object obj = null)
		{
			//
			// save settings and reset meter
			//

			// update selected sensors propoerty of setting object
			Settings.SensorRecords = TemperatureMeter.GetSensorRecords();
			Settings.Save();

			// update autostart setting
			UpdateAutoStartSetting();
		}
		void _LoadAndReset(object obj = null)
		{
			//
			// load settings from file and reset meter
			//

			// load settings 
			Settings.Reload();
			if (Settings.SensorRecords == null) Settings.SensorRecords = new List<SensorRecord>();

			// create meter
			TemperatureMeter = new TemperatureMeter();

			// update meter's sensor settings
			TemperatureMeter.SetSensorRecords(Settings.SensorRecords);

			// update autostart setting
			UpdateAutoStartSetting();
		}
		void _CancelSettingsUpdate(object obj = null)
		{
			//
			// Cancle setting update
			//

			// reload settings from storage
			Settings.Reload();
			if (Settings.SensorRecords == null) Settings.SensorRecords = new List<SensorRecord>();

			// update meter's sensor settings
			TemperatureMeter.SetSensorRecords(Settings.SensorRecords);

			// raised property changed event as these two values may be updated with underlying settings
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WarningThreshold"));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DangerThreshold"));
		}
		void _RescanSystem(object obj = null)
		{
			// create new temperature meter
			TemperatureMeter = new TemperatureMeter();

			// update meter's sensor settings
			TemperatureMeter.SetSensorRecords(Settings.SensorRecords);
		}
		void UpdateAutoStartSetting()
		{
			if (Settings.RunAtStartup)
			{
				AddAutoStartSchedule();
			}
			else
			{
				RemoveAutoStartSchedule();
			}
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

				// make sure start the task if is on battery
				td.Settings.DisallowStartIfOnBatteries = false;
				td.Settings.StopIfGoingOnBatteries = false;

				// Create an action
				string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
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
