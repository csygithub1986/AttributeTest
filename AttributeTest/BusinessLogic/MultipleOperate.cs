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
    public class MultipleOperate
    {
        //被操作对象
        public Type EquipmentType { get; set; }

        //操作类型
        public EMultipleOperateType OperateType { get; set; }

        ////是否是单步操作（对应流程化多步操作）
        //public bool IsSingleOperate { get; }

        //public SingleOperate()
        //{
        //    switch (OperateType)
        //    {
        //        case SingleOperateType.验电:
        //        case SingleOperateType.分闸:
        //        case SingleOperateType.合闸:
        //            IsSingleOperate = true;
        //            break;
        //        case SingleOperateType.断电:
        //        case SingleOperateType.送电:
        //            IsSingleOperate = false;
        //            break;
        //    }

        //}

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is MultipleOperate)) return false;
            MultipleOperate ope = obj as MultipleOperate;
            return ope.EquipmentType == EquipmentType && ope.OperateType == OperateType;
        }

        public override int GetHashCode()
        {
            return EquipmentType.GetHashCode() + OperateType.GetHashCode();
        }
    }
}
