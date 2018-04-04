using AttributeTest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest
{
    /// <summary>
    /// 操作类
    /// </summary>
    public class SingleOperate : ModelBase
    {
        //被操作对象
        public Type EquipmentType { get; set; }

        //public string EquipmentMark
        //{
        //    get
        //    {
        //        object[] attrs = EquipmentType?.GetCustomAttributes(typeof(OperationConfigMarkAttribute), false);
        //        if (attrs != null && attrs.Length == 1)
        //        {
        //            return (attrs[0] as OperationConfigMarkAttribute)?.Mark;
        //        }
        //        return string.Empty;
        //    }
        //}
        public string EquipmentMark { get; set; }

        //操作类型
        public ESingleOperateType OperateType { get; set; }



        /// <summary>
        /// UI辅助属性。该步骤是否被选为批操作中的一步
        /// </summary>
        public bool IsSelected { get { return _IsSelected; } set { if (_IsSelected != value) { _IsSelected = value; OnNotifyPropertyChanged("IsSelected"); } } }
        private bool _IsSelected;


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SingleOperate)) return false;
            SingleOperate ope = obj as SingleOperate;
            return ope.EquipmentType == EquipmentType && ope.OperateType == OperateType;
        }

        public override int GetHashCode()
        {
            return EquipmentType.GetHashCode() + OperateType.GetHashCode();
        }
    }
}
