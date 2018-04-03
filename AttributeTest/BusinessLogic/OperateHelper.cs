using AttributeTest.Attributes;
using AttributeTest.Ex;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest
{
    /// <summary>
    /// 操作类
    /// </summary>
    public class OperateHelper : ModelBase
    {
        private OperateHelper()
        {
            SingleOperConditionDic = new ObservableDictionary<SingleOperate, ObservableCollection<OperateCondition>>();

            #region 备用
            ////构造接地设备合闸条件
            //{
            //    SingleOperate so1 = new SingleOperate() { EquipmentType = typeof(Device), OperateType = SingleOperateType.合闸 };
            //    List<OperateCondition> ocList1 = new List<OperateCondition>();
            //    ocList1.Add(new OperateCondition() { PropertyMark = "合闸", PropertyName = "IsClosed", PropertyType = typeof(Switch).GetProperty("IsClosed").PropertyType, PropertyValue = false });
            //    OperateCondition oc1 = new OperateCondition() { PropertyMark = "隔离开关", PropertyType = typeof(Device).GetProperty("SwitchsRelated").PropertyType, PropertyName = "SwitchsRelated", RelatedEquipProList = ocList1 };
            //    OperateCondition oc2 = new OperateCondition() { PropertyMark = "合闸", PropertyType = typeof(Device).GetProperty("IsClosed").PropertyType, PropertyName = "IsClosed", PropertyValue = false };
            //    OperateCondition oc3 = new OperateCondition() { PropertyMark = "闭锁", PropertyType = typeof(Device).GetProperty("IsLocked").PropertyType, PropertyName = "IsLocked", PropertyValue = false };
            //    OperateCondition oc4 = new OperateCondition() { PropertyMark = "告警", PropertyType = typeof(Device).GetProperty("IsAlarming").PropertyType, PropertyName = "IsAlarming", PropertyValue = false };
            //    OperateCondition oc5 = new OperateCondition() { PropertyMark = "通过验电", PropertyType = typeof(Device).GetProperty("IsElecChecked").PropertyType, PropertyName = "IsElecChecked", PropertyValue = true };

            //    ObservableCollection<OperateCondition> list1 = new ObservableCollection<OperateCondition>();
            //    list1.Add(oc1);
            //    list1.Add(oc2);
            //    list1.Add(oc3);
            //    list1.Add(oc4);
            //    list1.Add(oc5);
            //    SingleOperConditionDic.Add(so1, list1);
            //}
            ////构造接地设备分闸条件
            //{
            //    SingleOperate so1 = new SingleOperate() { EquipmentType = typeof(Device), OperateType = SingleOperateType.分闸 };
            //    List<OperateCondition> ocList1 = new List<OperateCondition>();
            //    ocList1.Add(new OperateCondition() { PropertyMark = "门状态", PropertyName = "DoorState", PropertyType = typeof(Door).GetProperty("DoorState").PropertyType, PropertyValue = DoorState.关门 });
            //    OperateCondition oc1 = new OperateCondition() { PropertyMark = "门禁", PropertyType = typeof(Device).GetProperty("DoorsRelated").PropertyType, PropertyName = "DoorsRelated", RelatedEquipProList = ocList1 };
            //    OperateCondition oc2 = new OperateCondition() { PropertyMark = "合闸", PropertyType = typeof(Device).GetProperty("IsClosed").PropertyType, PropertyName = "IsClosed", PropertyValue = true };
            //    OperateCondition oc3 = new OperateCondition() { PropertyMark = "闭锁", PropertyType = typeof(Device).GetProperty("IsLocked").PropertyType, PropertyName = "IsLocked", PropertyValue = false };
            //    OperateCondition oc4 = new OperateCondition() { PropertyMark = "告警", PropertyType = typeof(Device).GetProperty("IsAlarming").PropertyType, PropertyName = "IsAlarming", PropertyValue = false };
            //    //OperateCondition oc5 = new OperateCondition() { PropertyType = typeof(Device).GetProperty("SwitchsRelated").PropertyType, PropertyName = "IsElecChecked", PropertyValue = true };//分闸不需要验电

            //    ObservableCollection<OperateCondition> list1 = new ObservableCollection<OperateCondition>();
            //    list1.Add(oc1);
            //    list1.Add(oc2);
            //    list1.Add(oc3);
            //    list1.Add(oc4);
            //    //list1.Add(oc5);
            //    SingleOperConditionDic.Add(so1, list1);
            //}
            #endregion


            //构造设备类基础元数据
            Type baseType = typeof(EquipmentBase);
            Type[] equipTypes = baseType.Assembly.GetTypes();
            foreach (var equipType in equipTypes)
            {
                if (equipType.IsSubclassOf(baseType))//是EquipmentBase的子类
                {
                    var operAttr = equipType.GetCustomAttribute<OperateTypeAttribute>(false);
                    if (operAttr != null)
                    {
                        if (operAttr.OperateType == typeof(ESingleOperateType))
                        {
                            foreach (var operType in operAttr.OperateTypeList)
                            {
                                SingleOperate so = new SingleOperate() { EquipmentType = equipType, OperateType = (ESingleOperateType)operType };
                                SingleOperConditionDic.Add(so, new ObservableCollection<OperateCondition>());
                            }
                        }
                        else if (operAttr.OperateType == typeof(EMultipleOperateType))
                        {
                            foreach (var operType in operAttr.OperateTypeList)
                            {
                                MultipleOperate mo = new MultipleOperate() { EquipmentType = equipType, OperateType = (EMultipleOperateType)operType };
                                MultiOperConditionDic.Add(mo, new ObservableCollection<SingleOperate>());
                            }
                        }
                    }

                    EquipmentSetupDic.Add(equipType.Name, new EquipmentSetup()
                    {
                        ClassName = equipType.Name,
                        IsOperatable = operAttr != null,
                        EquipmentName = equipType.GetCustomAttribute<OperationConfigMarkAttribute>(false)?.Mark,
                        //IsReal = false,
                        IsMultiStepOperation = operAttr.OperateType == typeof(EMultipleOperateType),
                        ClassFullName = equipType.FullName,
                        AssemblyName = equipType.Assembly.FullName,
                        ClassType= equipType
                    });

                }
            }




        }
        private static OperateHelper _Instance;
        public static OperateHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new OperateHelper();
                }
                return _Instance;
            }
        }

        //貌似可以用ObservableCollection，而不用ObservableDictionary（用于股道图绑定，需要Dictionary）
        public ObservableDictionary<string, EquipmentSetup> EquipmentSetupDic { get { return _EquipmentSetupDic; } set { if (_EquipmentSetupDic != value) { _EquipmentSetupDic = value; OnNotifyPropertyChanged("EquipmentSetupDic"); } } }
        private ObservableDictionary<string, EquipmentSetup> _EquipmentSetupDic = new ObservableDictionary<string, EquipmentSetup>();

        //一次单步操作，需要哪些设备类型的哪些条件（如果value为空集合，可以操作）
        public ObservableDictionary<SingleOperate, ObservableCollection<OperateCondition>> SingleOperConditionDic { get { return _SingleOperConditionDic; } set { if (_SingleOperConditionDic != value) { _SingleOperConditionDic = value; OnNotifyPropertyChanged("SingleOperConditionDic"); } } }
        private ObservableDictionary<SingleOperate, ObservableCollection<OperateCondition>> _SingleOperConditionDic = new ObservableDictionary<SingleOperate, ObservableCollection<OperateCondition>>();

        ////多步操作步骤
        //public ObservableDictionary<MultipleOperate, ObservableCollection<SingleOperate>> MultiOperStepDic { get { return _MultiOperStepDic; } set { if (_MultiOperStepDic != value) { _MultiOperStepDic = value; OnNotifyPropertyChanged("MultiOperStepDic"); } } }
        //private ObservableDictionary<MultipleOperate, ObservableCollection<SingleOperate>> _MultiOperStepDic;

        //多步操作条件（如果value为空集合，不能操作）
        public ObservableDictionary<MultipleOperate, ObservableCollection<SingleOperate>> MultiOperConditionDic { get { return _MultiOperConditionDic; } set { if (_MultiOperConditionDic != value) { _MultiOperConditionDic = value; OnNotifyPropertyChanged("MultiOperConditionDic"); } } }
        private ObservableDictionary<MultipleOperate, ObservableCollection<SingleOperate>> _MultiOperConditionDic = new ObservableDictionary<MultipleOperate, ObservableCollection<SingleOperate>>();


        public bool CanExecute(object param, ESingleOperateType operType)
        {
            if (!(param is EquipmentBase)) return false;
            EquipmentBase equip = param as EquipmentBase;
            SingleOperate so = new SingleOperate() { EquipmentType = equip.GetType(), OperateType = operType };

            ObservableCollection<OperateCondition> operateConditionList = null;
            SingleOperConditionDic.TryGetValue(so, out operateConditionList);
            if (operateConditionList == null) return false;//（重要）没有操作条件的记录，不是没有限制随便操作，而是不能操作。没有限制的操作ConditionList为空集合而非没有

            foreach (var operCondition in operateConditionList)
            {
                //找需要操作的设备中关联的设备
                Type deviceType = equip.GetType();
                PropertyInfo[] propertyInfos = deviceType.GetProperties();
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.Name == operCondition.PropertyName)
                    {
                        if (propertyInfo.PropertyType.IsValueType) //也可用 if (operCondition.PropertyValue != null)
                        {
                            if (propertyInfo.GetValue(equip).Equals(operCondition.PropertyValue) == false)//这里!=有问题，为什么？两个false不相等
                            {
                                return false;
                            }
                        }
                        else if (propertyInfo.PropertyType.IsSubclassOf(typeof(EquipmentBase)))//如果是单设备
                        {
                            var relatedEquip = propertyInfo.GetValue(equip); //实际上必须是EquipmentBase子类
                            foreach (var relatedProp in propertyInfo.PropertyType.GetProperties())
                            {
                                foreach (var pair in operCondition.RelatedEquipProList)
                                {
                                    if (pair.PropertyName == relatedProp.Name)
                                    {
                                        if (pair.PropertyValue.Equals(relatedProp.GetValue(relatedEquip)) == false)
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                        //else if (propertyInfo.PropertyType.IsAssignableFrom(typeof(ICollection)))//为什么不行
                        else if (propertyInfo.GetValue(equip) is ICollection)//如果是集合类
                        {
                            ICollection relatedEquips = (ICollection)propertyInfo.GetValue(equip); //关联的设备
                            foreach (var relatedEquip in relatedEquips)
                            {
                                foreach (var relatedProp in relatedEquip.GetType().GetProperties())
                                {
                                    foreach (var pair in operCondition.RelatedEquipProList)
                                    {
                                        if (pair.PropertyName == relatedProp.Name)
                                        {
                                            if (pair.PropertyValue.Equals(relatedProp.GetValue(relatedEquip)) == false)
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool CanExecute(EquipmentBase equipment, MultipleOperate operateType)
        {
            return false;
        }

    }
}
