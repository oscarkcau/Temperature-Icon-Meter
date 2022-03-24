using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace TemperatureIconMeterWPF
{
    class SensorTreeNode : INotifyPropertyChanged
    {
        // private field
        readonly ISensor sensor;
        readonly HardwareTreeNode _parent;
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
            if (sensor.Min.HasValue) Min = (float)Math.Round(sensor.Min.Value, 2);
            if (sensor.Max.HasValue) Max = (float)Math.Round(sensor.Max.Value, 2);
            if (sensor.Value.HasValue) Value = (float)Math.Round(sensor.Value.Value, 2);
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
}
