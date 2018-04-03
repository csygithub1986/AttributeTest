using AttributeTest.Ex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using AttributeTest.Attributes;

namespace AttributeTest
{
    public class SingleOperConfigVM : ViewModelBase
    {
        #region 属性
        public Dictionary<EquipmentSetup, Dictionary<SingleOperate, Dictionary<string, List<OperateCondition>>>> SetupDataSource { get { return _SetupDataSource; } set { if (_SetupDataSource != value) { _SetupDataSource = value; OnNotifyPropertyChanged("SetupDataSource"); } } }
        private Dictionary<EquipmentSetup, Dictionary<SingleOperate, Dictionary<string, List<OperateCondition>>>> _SetupDataSource;

        public List<EquipmentSetup> EquipmentSetups { get { return _EquipmentSetups; } set { if (_EquipmentSetups != value) { _EquipmentSetups = value; OnNotifyPropertyChanged("EquipmentSetups"); } } }
        private List<EquipmentSetup> _EquipmentSetups;

        public EquipmentSetup SelectedEquipSetup { get { return _SelectedEquipSetup; } set { if (_SelectedEquipSetup != value) { _SelectedEquipSetup = value; Operates = value == null ? null : SetupDataSource[value].Keys.ToList(); OnNotifyPropertyChanged("SelectedEquipSetup"); } } }
        private EquipmentSetup _SelectedEquipSetup;

        public List<SingleOperate> Operates { get { return _Operates; } set { if (_Operates != value) { _Operates = value; OnNotifyPropertyChanged("Operates"); } } }
        private List<SingleOperate> _Operates;

        public SingleOperate SelectedOperate { get { return _SelectedOperate; } set { if (_SelectedOperate != value) { _SelectedOperate = value; OperationConditions = value == null ? null : SetupDataSource[_SelectedEquipSetup][value]; OnNotifyPropertyChanged("SelectedOperate"); } } }
        private SingleOperate _SelectedOperate;

        public Dictionary<string, List<OperateCondition>> OperationConditions { get { return _OperationConditions; } set { if (_OperationConditions != value) { _OperationConditions = value; OnNotifyPropertyChanged("OperationConditions"); } } }
        private Dictionary<string, List<OperateCondition>> _OperationConditions;

        #endregion

        public SingleOperConfigVM()
        {
            EquipmentSetups = OperateHelper.Instance.EquipmentSetupDic.Where(p => p.Value.IsMultiStepOperation == false).Select(p => p.Value).ToList();
            AddToConditionCommand = new CommandBase(AddToCondition, null);
            DeleteConditionCommand = new CommandBase(DeleteCondition, null);
            InitDataSource();
        }

        private void InitDataSource()
        {
            var dataSource = new Dictionary<EquipmentSetup, Dictionary<SingleOperate, Dictionary<string, List<OperateCondition>>>>();
            foreach (var equipSetup in EquipmentSetups)
            {
                var dic1 = new Dictionary<SingleOperate, Dictionary<string, List<OperateCondition>>>();
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

                    dic1.Add(operate, tempAllConditions);
                }
                dataSource.Add(equipSetup, dic1);
            }
            SetupDataSource = dataSource;
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
