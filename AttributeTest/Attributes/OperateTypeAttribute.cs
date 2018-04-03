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
    sealed class OperateTypeAttribute : Attribute
    {
        public OperateTypeAttribute(ESingleOperateType type1, params ESingleOperateType[] restType)
        {
            OperateType = typeof(ESingleOperateType);
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

        public OperateTypeAttribute(EMultipleOperateType type1, params EMultipleOperateType[] restType)
        {
            OperateType = typeof(EMultipleOperateType);
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

        //public List<SingleOperateType> SingleOperateTypeList
        //{
        //    get { return _SingleOperateTypeList; }
        //}
        //readonly List<SingleOperateType> _SingleOperateTypeList = new List<SingleOperateType>();

        public List<Enum> OperateTypeList
        {
            get { return _OperateTypeList; }
        }
        readonly List<Enum> _OperateTypeList = new List<Enum>();

        public Type OperateType { get; }
    }
}
