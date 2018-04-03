using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest
{
    /// <summary>
    /// 操作条件类
    /// </summary>
    public class OperateCondition : ModelBase
    {
        //操作需要关联的设备类型
        //public Type RelatedEquipmentType { get; set; }

        public string PropertyName { get; set; }

        public string PropertyMark { get; set; }

        public Type PropertyType { get; set; }

        public ValueType PropertyValue { get { return _PropertyValue; } set { if (_PropertyValue != value) { _PropertyValue = value; OnNotifyPropertyChanged("PropertyValue"); } } }
        private ValueType _PropertyValue;


        //操作条件，即关联设备的属性和取值对
        public List<OperateCondition> RelatedEquipProList { get; set; }

        //是否被选定为条件（辅助UI操作）
        public bool IsChosen { get { return _IsChosen; } set { if (_IsChosen != value) { _IsChosen = value; OnNotifyPropertyChanged("IsChosen"); } } }
        private bool _IsChosen = false;

    }
}
