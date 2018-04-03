using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AttributeTest.Attributes;

namespace AttributeTest
{
    /// <summary>
    /// 股道
    /// </summary>
    [OperationConfigMark("股道")]
    [OperateType(EMultipleOperateType.断电, EMultipleOperateType.送电)]
    public class Track : EquipmentBase
    {
        [OperationConfigMark("配电状态")]//这个状态有点难办，它的计算方法要不要可配置
        public ETrackElecState TrackElecState { get { return _TrackElecState; } set { if (_TrackElecState != value) { _TrackElecState = value; OnNotifyPropertyChanged("TrackElecState"); } } }
        private ETrackElecState _TrackElecState = ETrackElecState.已断电;

        [OperationConfigMark("接地设备")]
        [OperationCondition]
        public ObservableCollection<Device> Devices { get { return _Devices; } set { if (_Devices != value) { _Devices = value; OnNotifyPropertyChanged("Devices"); } } }
        private ObservableCollection<Device> _Devices = new ObservableCollection<Device>();

        [OperationConfigMark("门禁")]
        [OperationCondition]
        public ObservableCollection<Door> Doors { get { return _Doors; } set { if (_Doors != value) { _Doors = value; OnNotifyPropertyChanged("Doors"); } } }
        private ObservableCollection<Door> _Doors = new ObservableCollection<Door>();

        [OperationConfigMark("隔离开关")]
        [OperationCondition]
        public ObservableCollection<Switch> Switchs { get { return _Switchs; } set { if (_Switchs != value) { _Switchs = value; OnNotifyPropertyChanged("Switchs"); } } }
        private ObservableCollection<Switch> _Switchs = new ObservableCollection<Switch>();

    }
}
