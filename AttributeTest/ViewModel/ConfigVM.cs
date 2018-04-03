using AttributeTest.Ex;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Collections;
using AttributeTest.Attributes;

namespace AttributeTest
{
    public partial class ConfigVM : ViewModelBase
    {
        #region 属性
        public Depot SystemDepot { get { return _SystemDepot; } set { if (_SystemDepot != value) { _SystemDepot = value; OnNotifyPropertyChanged("SystemDepot"); } } }
        private Depot _SystemDepot;

        //本页的缓存
        public ObservableDictionary<SingleOperate, ObservableCollection<OperateCondition>> SingleOperConditionDic { get { return _SingleOperConditionDic; } set { if (_SingleOperConditionDic != value) { _SingleOperConditionDic = value; OnNotifyPropertyChanged("SingleOperConditionDic"); } } }
        private ObservableDictionary<SingleOperate, ObservableCollection<OperateCondition>> _SingleOperConditionDic = new ObservableDictionary<SingleOperate, ObservableCollection<OperateCondition>>();

        //本页的缓存
        public ObservableDictionary<MultipleOperate, ObservableCollection<SingleOperate>> MultiOperConditionDic { get { return _MultiOperConditionDic; } set { if (_MultiOperConditionDic != value) { _MultiOperConditionDic = value; OnNotifyPropertyChanged("MultiOperConditionDic"); } } }
        private ObservableDictionary<MultipleOperate, ObservableCollection<SingleOperate>> _MultiOperConditionDic;

        //设备配置栏中的ItemsSource之一
        public ObservableDictionary<string, EquipmentSetup> EquipmentSetupDic { get { return _EquipmentSetupDic; } set { if (_EquipmentSetupDic != value) { _EquipmentSetupDic = value; OnNotifyPropertyChanged("EquipmentSetupDic"); } } }
        private ObservableDictionary<string, EquipmentSetup> _EquipmentSetupDic = new ObservableDictionary<string, EquipmentSetup>();

        //设备配置栏当中的选中项
        public EquipmentSetup SelectedEquipSetup
        {
            get { return _SelectedEquipSetup; }
            set
            {
                if (_SelectedEquipSetup != value)
                {
                    _SelectedEquipSetup = value;
                    OnSelectedEquipSetupChanged();
                    ////弃用。原设计将可操作性等纳入配置页。现在在类定义上通过特性配置
                    //if (value != null)
                    //{
                    //    value.PropertyChanged -= SelectedEquipSetup_PropertyChanged;
                    //    value.PropertyChanged += SelectedEquipSetup_PropertyChanged;
                    //}
                    OnNotifyPropertyChanged("SelectedEquipSetup");
                }
            }
        }
        private EquipmentSetup _SelectedEquipSetup;

        //操作配置栏中的ItemsSource之一
        public List<SingleOperate> SingleOperateList { get { return _SingleOperateList; } set { if (_SingleOperateList != value) { _SingleOperateList = value; OnNotifyPropertyChanged("SingleOperateList"); } } }
        private List<SingleOperate> _SingleOperateList;

        //操作配置栏中的ItemsSource之一
        public List<MultipleOperate> MultipleOperateList { get { return _MultipleOperateList; } set { if (_MultipleOperateList != value) { _MultipleOperateList = value; OnNotifyPropertyChanged("MultipleOperateList"); } } }
        private List<MultipleOperate> _MultipleOperateList;


        //选定的操作
        public object SelectedOperation { get { return _SelectedOperation; } set { if (_SelectedOperation != value) { _SelectedOperation = value; OnSelectedOperationChanged(); OnNotifyPropertyChanged("SelectedOperation"); } } }
        private object _SelectedOperation;


        public Visibility ConditionPanelVisibility { get { return _ConditionPanelVisibility; } set { if (_ConditionPanelVisibility != value) { _ConditionPanelVisibility = value; OnNotifyPropertyChanged("ConditionPanelVisibility"); } } }
        private Visibility _ConditionPanelVisibility = Visibility.Hidden;

        public Visibility OperStepPanelVisibility { get { return _OperStepPanelVisibility; } set { if (_OperStepPanelVisibility != value) { _OperStepPanelVisibility = value; OnNotifyPropertyChanged("OperStepPanelVisibility"); } } }
        private Visibility _OperStepPanelVisibility = Visibility.Hidden;


        //选定的配置栏数据
        //public Dictionary<string, List<OperateCondition>> ChosenOperationConditions { get { return _ChosenOperationConditions; } set { if (_ChosenOperationConditions != value) { _ChosenOperationConditions = value; OnNotifyPropertyChanged("ChosenOperationConditions"); } } }
        //private Dictionary<string, List<OperateCondition>> _ChosenOperationConditions;


        public Dictionary<string, List<OperateCondition>> AllOperationConditions { get { return _AllOperationConditions; } set { if (_AllOperationConditions != value) { _AllOperationConditions = value; OnNotifyPropertyChanged("AllOperationConditions"); } } }
        private Dictionary<string, List<OperateCondition>> _AllOperationConditions;

        //步骤配置栏数据

        #endregion

        public ConfigVM()
        {
            SystemData.Instance.PropertyChanged += SystemData_PropertyChanged;
            //OperateHelper.Instance.PropertyChanged += OperateHelper_PropertyChanged;

            EquipmentSetupDic = OperateHelper.Instance.EquipmentSetupDic;
            SingleOperConditionDic = OperateHelper.Instance.SingleOperConditionDic;
            MultiOperConditionDic = OperateHelper.Instance.MultiOperConditionDic;

            AddToConditionCommand = new CommandBase(AddToCondition, null);
            DeleteConditionCommand = new CommandBase(DeleteCondition, null);
        }



        #region 命令定义和执行
        public CommandBase AddToConditionCommand { get; set; }
        public CommandBase DeleteConditionCommand { get; set; }

        private void AddToCondition(object obj)
        {
            var c = obj as OperateCondition;
            c.IsChosen = true;
            c.PropertyValue = c.PropertyType.IsValueType ? Activator.CreateInstance(c.PropertyType) : null;
        }
        private void DeleteCondition(object obj)
        {
            var c = obj as OperateCondition;
            c.IsChosen = false;
            //c.PropertyValue = c.PropertyType.IsValueType ? Activator.CreateInstance(c.PropertyType) : null;
        }
        #endregion
    }
}
