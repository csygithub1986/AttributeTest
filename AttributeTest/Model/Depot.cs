using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttributeTest.Attributes;

namespace AttributeTest
{
    /// <summary>
    /// 车辆段
    /// </summary>
    [OperationConfigMark("车辆段")]
    public class Depot : ModelBase
    {
        [OperationConfigMark("股道")]
        public ObservableCollection<Track> Tracks { get { return _Tracks; } set { if (_Tracks != value) { _Tracks = value; OnNotifyPropertyChanged("Tracks"); } } }
        private ObservableCollection<Track> _Tracks;

        [OperationConfigMark("接地设备")]
        public ObservableCollection<Device> Devices { get { return _Devices; } set { if (_Devices != value) { _Devices = value; OnNotifyPropertyChanged("Devices"); } } }
        private ObservableCollection<Device> _Devices;

        [OperationConfigMark("门禁")]
        public ObservableCollection<Door> Doors { get { return _Doors; } set { if (_Doors != value) { _Doors = value; OnNotifyPropertyChanged("Doors"); } } }
        private ObservableCollection<Door> _Doors;

        [OperationConfigMark("隔离开关")]
        public ObservableCollection<Switch> Switchs { get { return _Switchs; } set { if (_Switchs != value) { _Switchs = value; OnNotifyPropertyChanged("Switchs"); } } }
        private ObservableCollection<Switch> _Switchs;

    }
}
