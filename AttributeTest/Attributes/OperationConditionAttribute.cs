using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest.Attributes
{
    /// <summary>
    /// 标注的属性要作为判断操作可否进行的条件，没标注的属性状态不影响设备操作
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class OperationConditionAttribute : Attribute
    {
    }
}
