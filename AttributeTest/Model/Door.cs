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
    /// 门禁
    /// </summary>
    [OperationConfigMark("门禁")]
    [OperateType(ESingleOperateType.开门, ESingleOperateType.关门)]
    public class Door : EquipmentBase
    {
        [OperationConfigMark("有人")]
        [OperationCondition]
        public bool HasPeople { get { return _HasPeople; } set { if (_HasPeople != value) { _HasPeople = value; OnNotifyPropertyChanged("HasPeople"); } } }
        private bool _HasPeople;

        [OperationConfigMark("门状态")]
        [OperationCondition]
        public EDoorState DoorState { get { return _DoorState; } set { if (_DoorState != value) { _DoorState = value; OnNotifyPropertyChanged("DoorState"); } } }
        private EDoorState _DoorState;

        [OperationConfigMark("接地设备")]
        [OperationCondition]
        public ObservableCollection<Device> DevicesRelated { get { return _DevicesRelated; } set { if (_DevicesRelated != value) { _DevicesRelated = value; OnNotifyPropertyChanged("DevicesRelated"); } } }
        private ObservableCollection<Device> _DevicesRelated = new ObservableCollection<Device>();
    }
}
