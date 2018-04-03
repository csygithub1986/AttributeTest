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
    /// 隔离开关
    /// </summary>
    [OperationConfigMark("隔离开关")]
    [OperateType(ESingleOperateType.分闸, ESingleOperateType.合闸)]
    public class Switch : EquipmentBase
    {
        [OperationConfigMark("接地设备")]
        [OperationCondition]
        public ObservableCollection<Device> DevicesRelated { get { return _DevicesRelated; } set { if (_DevicesRelated != value) { _DevicesRelated = value; OnNotifyPropertyChanged("DevicesRelated"); } } }
        private ObservableCollection<Device> _DevicesRelated = new ObservableCollection<Device>();

    }
}
