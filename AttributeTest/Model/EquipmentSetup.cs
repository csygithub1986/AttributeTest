using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest
{
    /// <summary>
    /// 系统基础设备配置
    /// </summary>
    public class EquipmentSetup : ModelBase
    {
        /// <summary>
        /// 程序集名称（暂未用）
        /// </summary>
        public string AssemblyName { get { return _AssemblyName; } set { if (_AssemblyName != value) { _AssemblyName = value; OnNotifyPropertyChanged("AssemblyName"); } } }
        private string _AssemblyName;


        public Type ClassType { get { return _ClassType; } set { if (_ClassType != value) { _ClassType = value; OnNotifyPropertyChanged("ClassType"); } } }
        private Type _ClassType;


        /// <summary>
        ///   类的全名，包括命名空间，但不包括程序集
        /// </summary>
        public string ClassFullName { get { return _ClassFullName; } set { if (_ClassFullName != value) { _ClassFullName = value; OnNotifyPropertyChanged("ClassFullName"); } } }
        private string _ClassFullName;

        /// <summary>
        /// 在代码中的类名
        /// </summary>
        public string ClassName { get { return _ClassName; } set { if (_ClassName != value) { _ClassName = value; OnNotifyPropertyChanged("ClassName"); } } }
        private string _ClassName;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get { return _EquipmentName; } set { if (_EquipmentName != value) { _EquipmentName = value; OnNotifyPropertyChanged("EquipmentName"); } } }
        private string _EquipmentName;

        /// <summary>
        /// 是否可通过软件操作
        /// </summary>
        public bool IsOperatable { get { return _IsOperatable; } set { if (_IsOperatable != value) { _IsOperatable = value; OnNotifyPropertyChanged("IsOperatable"); } } }
        private bool _IsOperatable;

        /// <summary>
        /// 是否是真是设备（是否需要？？）
        /// </summary>
        //public bool IsReal { get { return _IsReal; } set { if (_IsReal != value) { _IsReal = value; OnNotifyPropertyChanged("IsReal"); } } }
        //private bool _IsReal;

        /// <summary>
        /// 是否是多部操作（是否需要）
        /// </summary>
        public bool IsMultiStepOperation { get { return _IsMultiStepOperation; } set { if (_IsMultiStepOperation != value) { _IsMultiStepOperation = value; OnNotifyPropertyChanged("IsMultiStepOperation"); } } }
        private bool _IsMultiStepOperation;

    }

}
