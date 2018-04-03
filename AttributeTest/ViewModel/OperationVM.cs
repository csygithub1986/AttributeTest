using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AttributeTest
{
    public class OperationVM : ViewModelBase
    {
        public Depot SystemDepot { get { return _SystemDepot; } set { if (_SystemDepot != value) { _SystemDepot = value; OnNotifyPropertyChanged("SystemDepot"); } } }
        private Depot _SystemDepot;

        public OperationVM()
        {
            SystemData.Instance.PropertyChanged += SystemData_PropertyChanged;
            CloseCommand = new CommandBase(CloseAction, CanCloseExecute);
            OpenCommand = new CommandBase(OpenAction, CanOpenExecute);
        }

        #region 命令和命令执行
        public CommandBase CloseCommand { get; set; }
        public CommandBase OpenCommand { get; set; }

        private void CloseAction(object param)
        {
            EquipmentBase equip = param as EquipmentBase;
            if (equip == null) return;
            equip.IsClosed = true;
        }

        private void OpenAction(object param)
        {
            EquipmentBase equip = param as EquipmentBase;
            if (equip == null) return;
            equip.IsClosed = false;
        }

        private bool CanCloseExecute(object param)
        {
            return OperateHelper.Instance.CanExecute(param, ESingleOperateType.合闸);
        }

        private bool CanOpenExecute(object param)
        {
            return OperateHelper.Instance.CanExecute(param, ESingleOperateType.分闸);
        }

        #endregion


        private void SystemData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SystemDepot")
            {
                SystemDepot = SystemData.Instance.SystemDepot;
            }
        }

    }
}
