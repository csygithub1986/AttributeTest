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

        public object PropertyValue { get; set; }


        //操作条件，即关联设备的属性和取值对(string 为PropertyInfo的Name，有可能是bool等基础类型，有可能是equipment对象，或者集合)
        //public Dictionary<string, object> RelatedEquipPropDic { get; set; }//TODO:弃用

        //操作条件，即关联设备的属性和取值对
        public List<OperateCondition> RelatedEquipProList { get; set; }

        //是否被选定为条件（辅助UI操作）
        public bool IsChosen { get { return _IsChosen; } set { if (_IsChosen != value) { _IsChosen = value; OnNotifyPropertyChanged("IsChosen"); } } }
        private bool _IsChosen = false;

    }
}
