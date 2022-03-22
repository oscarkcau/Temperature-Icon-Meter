using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace TemperatureIconMeterWPF
{
    class HardwareTreeNode
    {
        // private field
        readonly IHardware hardware;

        // public properties
        public string Name { get => hardware.Name; }
        public ObservableCollection<SensorTreeNode> Sensors { get; private set; }

        // constructor
        public HardwareTreeNode(IHardware hardware)
        {
            this.hardware = hardware;
            this.Sensors = new ObservableCollection<SensorTreeNode>();
        }

        // publid method
        public void Update() { hardware.Update(); }
    }
}
