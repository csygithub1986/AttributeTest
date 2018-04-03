using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest.Attributes
{
    /// <summary>
    /// 配置设备类可进行的单设备操作类型，如果不配置则为不可进行单设备操作
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class MultipleOperateTypeAttribute : Attribute
    {
        public MultipleOperateTypeAttribute(MultipleOperateType type1, params MultipleOperateType[] restType)
        {
            _OperateTypeList.Add(type1);
            if (restType != null)
            {
                foreach (var item in restType)
                {
                    if (!_OperateTypeList.Contains(item))
                    {
                        _OperateTypeList.Add(item);
                    }
                }
            }
        }

        public List<MultipleOperateType> OperateTypeList
        {
            get { return _OperateTypeList; }
        }
        readonly List<MultipleOperateType> _OperateTypeList = new List<MultipleOperateType>();
    }
}
