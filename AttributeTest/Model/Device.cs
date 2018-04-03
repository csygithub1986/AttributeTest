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
    /// 设备类
    /// </summary>
    [OperationConfigMark("接地设备")]
    [OperateType(ESingleOperateType.分闸, ESingleOperateType.合闸, ESingleOperateType.验电)]
    public class Device : EquipmentBase
    {
        [OperationConfigMark("闭锁")]
        [OperationCondition]
        public bool IsLocked { get { return _IsLocked; } set { if (_IsLocked != value) { _IsLocked = value; OnNotifyPropertyChanged("IsLocked"); } } }
        private bool _IsLocked;

        [OperationConfigMark("告警")]
        [OperationCondition]
        public bool IsAlarming { get { return _IsAlarming; } set { if (_IsAlarming != value) { _IsAlarming = value; OnNotifyPropertyChanged("IsAlarming"); } } }
        private bool _IsAlarming;

        [OperationConfigMark("已验电")]
        [OperationCondition]
        public bool IsElecChecked { get { return _IsElecChecked; } set { if (_IsElecChecked != value) { _IsElecChecked = value; OnNotifyPropertyChanged("IsElecChecked"); } } }
        private bool _IsElecChecked;

        [OperationConfigMark("门禁")]
        [OperationCondition]
        public ObservableCollection<Door> DoorsRelated { get { return _DoorsRelated; } set { if (_DoorsRelated != value) { _DoorsRelated = value; OnNotifyPropertyChanged("DoorsRelated"); } } }
        private ObservableCollection<Door> _DoorsRelated = new ObservableCollection<Door>();

        [OperationConfigMark("隔离开关")]
        [OperationCondition]
        public ObservableCollection<Switch> SwitchsRelated { get { return _SwitchsRelated; } set { if (_SwitchsRelated != value) { _SwitchsRelated = value; OnNotifyPropertyChanged("SwitchsRelated"); } } }
        private ObservableCollection<Switch> _SwitchsRelated = new ObservableCollection<Switch>();

    }
}
