using AttributeTest.Ex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using AttributeTest.Attributes;
using System.Collections.ObjectModel;

namespace AttributeTest
{
    public class SingleOperConfigVM : ViewModelBase
    {
        #region 属性
        public Dictionary<EquipmentSetup, Dictionary<SingleOperate, Dictionary<string, List<OperateCondition>>>> SingleOperateDataSource { get { return _SingleOperateDataSource; } set { if (_SingleOperateDataSource != value) { _SingleOperateDataSource = value; OnNotifyPropertyChanged("SingleOperateDataSource"); } } }
        private Dictionary<EquipmentSetup, Dictionary<SingleOperate, Dictionary<string, List<OperateCondition>>>> _SingleOperateDataSource;

        public Dictionary<EquipmentSetup, Dictionary<MultipleOperate, List<SingleOperate>>> MultipleOperateDataSource { get { return _MultipleOperateDataSource; } set { if (_MultipleOperateDataSource != value) { _MultipleOperateDataSource = value; OnNotifyPropertyChanged("MultipleOperateDataSource"); } } }
        private Dictionary<EquipmentSetup, Dictionary<MultipleOperate, List<SingleOperate>>> _MultipleOperateDataSource;


        public List<EquipmentSetup> EquipmentSetups { get { return _EquipmentSetups; } set { if (_EquipmentSetups != value) { _EquipmentSetups = value; OnNotifyPropertyChanged("EquipmentSetups"); } } }
        private List<EquipmentSetup> _EquipmentSetups;

        public EquipmentSetup SelectedEquipSetup { get { return _SelectedEquipSetup; } set { if (_SelectedEquipSetup != value) { _SelectedEquipSetup = value; OnSelectedEquipSetupChanged(); OnNotifyPropertyChanged("SelectedEquipSetup"); } } }
        private EquipmentSetup _SelectedEquipSetup;


        public List<SingleOperate> SingleOperates { get { return _SingleOperates; } set { if (_SingleOperates != value) { _SingleOperates = value; OnNotifyPropertyChanged("SingleOperates"); } } }
        private List<SingleOperate> _SingleOperates;

        public SingleOperate SelectedSingleOperate { get { return _SelectedSingleOperate; } set { if (_SelectedSingleOperate != value) { _SelectedSingleOperate = value; OperationConditions = value == null ? null : SingleOperateDataSource[_SelectedEquipSetup][value]; OnNotifyPropertyChanged("SelectedSingleOperate"); } } }
        private SingleOperate _SelectedSingleOperate;

        public List<MultipleOperate> MultipleOperates { get { return _MultipleOperates; } set { if (_MultipleOperates != value) { _MultipleOperates = value; OnNotifyPropertyChanged("MultipleOperates"); } } }
        private List<MultipleOperate> _MultipleOperates;

        public MultipleOperate SelectedMultipleOperate { get { return _SelectedMultipleOperate; } set { if (_SelectedMultipleOperate != value) { _SelectedMultipleOperate = value; OperationSteps = value == null ? null : new ObservableCollection<SingleOperate>(MultipleOperateDataSource[_SelectedEquipSetup][value]); OnNotifyPropertyChanged("SelectedMultipleOperate"); } } }
        private MultipleOperate _SelectedMultipleOperate;


        public Dictionary<string, List<OperateCondition>> OperationConditions { get { return _OperationConditions; } set { if (_OperationConditions != value) { _OperationConditions = value; OnNotifyPropertyChanged("OperationConditions"); } } }
        private Dictionary<string, List<OperateCondition>> _OperationConditions;


        public ObservableCollection<SingleOperate> OperationSteps { get { return _OperationSteps; } set { if (_OperationSteps != value) { _OperationSteps = value; OnNotifyPropertyChanged("OperationSteps"); } } }
        private ObservableCollection<SingleOperate> _OperationSteps;

        #endregion

        public SingleOperConfigVM()
        {
            EquipmentSetups = OperateHelper.Instance.EquipmentSetupDic.Select(p => p.Value).ToList();//.Where(p => p.Value.IsMultiStepOperation == false).Select(p => p.Value).ToList();
            AddToConditionCommand = new CommandBase(AddToCondition, null);
            DeleteConditionCommand = new CommandBase(DeleteCondition, null);
            InitSingleOperDataSource();
            InitMultipleOperDataSource();
        }

        private void InitSingleOperDataSource()
        {
            var dataSource = new Dictionary<EquipmentSetup, Dictionary<SingleOperate, Dictionary<string, List<OperateCondition>>>>();
            foreach (var equipSetup in EquipmentSetups)
            {
                if (equipSetup.IsMultiStepOperation) continue;
                var tempDic = new Dictionary<SingleOperate, Dictionary<string, List<OperateCondition>>>();
                var operateList = OperateHelper.Instance.SingleOperConditionDic.Keys.Where(p => p.EquipmentType.Name == equipSetup.ClassName).ToList();
                foreach (var operate in operateList)
                {
                    var tempAllConditions = new Dictionary<string, List<OperateCondition>>();
                    var equipProps = equipSetup.ClassType.GetProperties();
                    foreach (var equipProp in equipProps)
                    {
                        if (equipProp.GetCustomAttributes(typeof(OperationConditionAttribute), false).Length == 1)
                        {
                            string mark = (equipProp.GetCustomAttributes(typeof(OperationConfigMarkAttribute), false).FirstOrDefault() as OperationConfigMarkAttribute)?.Mark;
                            if (equipProp.PropertyType.IsValueType)//值类型，本设备属性
                            {
                                tempAllConditions.AddOrUpdateList("本设备", new OperateCondition() { PropertyName = equipProp.Name, PropertyMark = mark, PropertyType = equipProp.PropertyType });
                            }
                            else
                            {
                                Type relatedEquipType = null;
                                if (equipProp.PropertyType.IsSubclassOf(typeof(EquipmentBase)))
                                {
                                    relatedEquipType = equipProp.PropertyType;
                                }
                                else if (equipProp.PropertyType.GenericTypeArguments.Length == 1 && (typeof(IList).IsAssignableFrom(equipProp.PropertyType)) && equipProp.PropertyType.GenericTypeArguments[0].IsSubclassOf(typeof(EquipmentBase)))
                                {
                                    relatedEquipType = equipProp.PropertyType.GenericTypeArguments[0];
                                }
                                if (relatedEquipType == null) continue;
                                //获得关联设备的所有属性
                                List<OperateCondition> relatedConditionList = new List<OperateCondition>();
                                var relatedEquipProps = equipSetup.ClassType.GetProperties();
                                foreach (var relatedProp in relatedEquipProps)
                                {
                                    if (relatedProp.GetCustomAttributes(typeof(OperationConditionAttribute), false).Length == 1)
                                    {
                                        string relatedChildMark = (relatedProp.GetCustomAttributes(typeof(OperationConfigMarkAttribute), false).FirstOrDefault() as OperationConfigMarkAttribute)?.Mark;
                                        if (relatedProp.PropertyType.IsValueType)//值类型
                                        {
                                            relatedConditionList.Add(new OperateCondition() { PropertyName = relatedProp.Name, PropertyMark = relatedChildMark, PropertyType = relatedProp.PropertyType });
                                        }
                                    }
                                }
                                string relatedProMark = (equipProp.GetCustomAttributes(typeof(OperationConfigMarkAttribute), false).FirstOrDefault() as OperationConfigMarkAttribute)?.Mark;
                                tempAllConditions.AddOrUpdateListRange(relatedProMark, relatedConditionList);
                            }
                        }
                    }
                    //复制已经有的属性值
                    var conditionsData = OperateHelper.Instance.SingleOperConditionDic[new SingleOperate() { EquipmentType = equipSetup.ClassType, OperateType = operate.OperateType }];
                    foreach (var cd in conditionsData)
                    {
                        string key = null;
                        if (cd.PropertyType.IsValueType)//值类型，本设备属性
                        {
                            key = "本设备";
                        }
                        //是EquipmentBase子类或是EquipmentBase的泛型集合（这样判断就是怕设备类的属性特性标注有误），关联设备属性
                        else if (cd.PropertyType.IsSubclassOf(typeof(EquipmentBase)) || (cd.PropertyType.GenericTypeArguments.Length == 1 &&
                                  (typeof(IList).IsAssignableFrom(cd.PropertyType)) && cd.PropertyType.GenericTypeArguments[0].IsSubclassOf(typeof(EquipmentBase))))
                        {
                            key = cd.PropertyName;
                        }
                        if (key != null)
                        {
                            List<OperateCondition> tempList = null;
                            tempAllConditions.TryGetValue(cd.PropertyName, out tempList);
                            if (tempList != null)
                            {
                                var op = tempList.FirstOrDefault(p => p.PropertyName == cd.PropertyName);
                                if (op != null)
                                {
                                    op.PropertyValue = cd.PropertyValue;
                                    op.IsChosen = true;
                                }
                            }
                        }
                    }

                    tempDic.Add(operate, tempAllConditions);
                }
                dataSource.Add(equipSetup, tempDic);
            }
            SingleOperateDataSource = dataSource;
        }

        private void InitMultipleOperDataSource()
        {
            var dataSource = new Dictionary<EquipmentSetup, Dictionary<MultipleOperate, List<SingleOperate>>>();
            foreach (var equipSetup in EquipmentSetups)
            {
                if (!equipSetup.IsMultiStepOperation) continue;
                var tempDic = new Dictionary<MultipleOperate, List<SingleOperate>>();
                var operateList = OperateHelper.Instance.MultiOperConditionDic.Keys.Where(p => p.EquipmentType.Name == equipSetup.ClassName).ToList();
                foreach (var operate in operateList)
                {
                    var stepList = new List<SingleOperate>();
                    var equipProps = equipSetup.ClassType.GetProperties();
                    foreach (var equipProp in equipProps)
                    {
                        if (equipProp.GetCustomAttributes(typeof(OperationConditionAttribute), false).Length == 1)
                        {
                            string mark = (equipProp.GetCustomAttributes(typeof(OperationConfigMarkAttribute), false).FirstOrDefault() as OperationConfigMarkAttribute)?.Mark;

                            Type relatedEquipType = null;
                            if (equipProp.PropertyType.IsSubclassOf(typeof(EquipmentBase)))
                            {
                                relatedEquipType = equipProp.PropertyType;
                            }
                            else if (equipProp.PropertyType.GenericTypeArguments.Length == 1 && (typeof(IList).IsAssignableFrom(equipProp.PropertyType)) && equipProp.PropertyType.GenericTypeArguments[0].IsSubclassOf(typeof(EquipmentBase)))
                            {
                                relatedEquipType = equipProp.PropertyType.GenericTypeArguments[0];
                            }
                            else continue;
                            if (relatedEquipType == null) continue;
                            //获得关联设备的所有属性（TODO：根据之前的配置排序）
                            SingleOperate mo = new SingleOperate() { EquipmentType = relatedEquipType, OperateType = default(ESingleOperateType), EquipmentMark = mark };
                            stepList.Add(mo);
                        }
                    }
                    //复制已经有的属性值
                    var singleSteps = OperateHelper.Instance.MultiOperConditionDic[new MultipleOperate() { EquipmentType = equipSetup.ClassType, OperateType = operate.OperateType }];
                    foreach (var singleOper in singleSteps)
                    {
                        var oper = stepList.FirstOrDefault(p => p.EquipmentType == singleOper.EquipmentType);
                        if (oper != null)
                        {
                            oper.IsSelected = singleOper.IsSelected;
                            oper.OperateType = singleOper.OperateType;
                        }
                    }
                    tempDic.Add(operate, stepList);
                }
                dataSource.Add(equipSetup, tempDic);
            }
            MultipleOperateDataSource = dataSource;
        }

        private void OnSelectedEquipSetupChanged()
        {
            if (_SelectedEquipSetup == null)
            {
                SingleOperates = null;
                MultipleOperates = null;
            }
            if (_SelectedEquipSetup.IsMultiStepOperation == false)
            {
                SingleOperates = SingleOperateDataSource[_SelectedEquipSetup].Keys.ToList();
            }
            else
            {
                MultipleOperates = MultipleOperateDataSource[_SelectedEquipSetup].Keys.ToList();
            }
        }


        #region 命令定义和执行
        public CommandBase AddToConditionCommand { get; set; }
        public CommandBase DeleteConditionCommand { get; set; }

        private void AddToCondition(object obj)
        {
            var c = obj as OperateCondition;
            c.IsChosen = true;
            c.PropertyValue = (ValueType)Activator.CreateInstance(c.PropertyType);
        }
        private void DeleteCondition(object obj)
        {
            var c = obj as OperateCondition;
            c.IsChosen = false;
        }
        #endregion
    }
}
