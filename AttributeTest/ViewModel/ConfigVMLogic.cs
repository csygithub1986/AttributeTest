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
        #region 事件处理
        private void SystemData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SystemDepot")
            {
                SystemDepot = SystemData.Instance.SystemDepot;
            }
        }

        private void OnSelectedEquipSetupChanged()
        {

            //if (SelectedEquipSetup.IsOperatable) //弃用。原设计将可操作性等纳入配置页。这里肯定是可操作的
            if (!SelectedEquipSetup.IsMultiStepOperation)//弃用。原设计将可操作类型等纳入配置页。这里操作类型已经决定了
            {
                var addedTypes = SingleOperConditionDic.Keys.Where(p => p.EquipmentType.Name == SelectedEquipSetup.ClassName).ToList();
                //if (addedTypes.Count == 0)
                //{
                //    var typesInSystem = (int[])Enum.GetValues(typeof(ESingleOperateType));
                //    foreach (int enumValue in typesInSystem)
                //    {
                //        SingleOperConditionDic.Add(new SingleOperate()
                //        {
                //            EquipmentType = Type.GetType(SelectedEquipSetup.ClassFullName, true, false),
                //            OperateType = (ESingleOperateType)enumValue
                //        }, new ObservableCollection<OperateCondition>());
                //    }
                //    addedTypes = SingleOperConditionDic.Keys.Where(p => p.EquipmentType.Name == SelectedEquipSetup.ClassName).ToList();
                //}
                SingleOperateList = addedTypes;
            }
            else
            {
                var addedTypes = MultiOperConditionDic.Keys.Where(p => p.EquipmentType.Name == SelectedEquipSetup.ClassName).ToList();
                //if (addedTypes.Count == 0)
                //{
                //    var typesInSystem = (int[])Enum.GetValues(typeof(EMultipleOperateType));
                //    foreach (int enumValue in typesInSystem)
                //    {
                //        MultiOperConditionDic.Add(new MultipleOperate()
                //        {
                //            EquipmentType = Type.GetType(SelectedEquipSetup.ClassFullName, true, false),
                //            OperateType = (EMultipleOperateType)enumValue
                //        }, new ObservableCollection<SingleOperate>());
                //    }
                //    addedTypes = MultiOperConditionDic.Keys.Where(p => p.EquipmentType.Name == SelectedEquipSetup.ClassName).ToList();
                //}
                MultipleOperateList = addedTypes;
            }
        }

        private void OnSelectedOperationChanged()
        {
            if (_SelectedOperation == null)
            {
                ConditionPanelVisibility = Visibility.Hidden;
                OperStepPanelVisibility = Visibility.Hidden;
            }
            else if (_SelectedOperation is SingleOperate)
            {
                ConditionPanelVisibility = Visibility.Visible;
                OperStepPanelVisibility = Visibility.Hidden;
                Type selectedEquipType = Type.GetType(SelectedEquipSetup.ClassFullName, true, false);

                //构造AllOperationConditions
                var tempAllConditions = new Dictionary<string, List<OperateCondition>>();
                var equipProps = selectedEquipType.GetProperties();
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
                            var relatedEquipProps = selectedEquipType.GetProperties();
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

                #region 弃用
                //构造ChosenOperationConditions
                var conditionsData = SingleOperConditionDic[new SingleOperate() { EquipmentType = selectedEquipType, OperateType = ((SingleOperate)_SelectedOperation).OperateType }];
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

                #endregion






                AllOperationConditions = tempAllConditions;

            }
            else if (_SelectedOperation is MultipleOperate)
            {
                ConditionPanelVisibility = Visibility.Hidden;
                OperStepPanelVisibility = Visibility.Visible;

            }
        }

        #endregion

    }
}
