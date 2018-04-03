using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest.Attributes
{
    /// <summary>
    /// 进行设备操作配置时，对类、属性的中文标注，以便界面显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class OperationConfigMarkAttribute : Attribute
    {
        public OperationConfigMarkAttribute(string mark)
        {
            this._Mark = mark;
        }
        public string Mark
        {
            get { return _Mark; }
        }
        readonly string _Mark;
    }
}
