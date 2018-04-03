using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AttributeTest
{
    public class CommandBase : ICommand
    {
        public CommandBase()
        {

        }

        public CommandBase(Action<object> execAction, Func<object, bool> canExecte)
        {
            ExecuteAction = execAction;
            CanExecuteFunc = canExecte;
        }

        private bool _canExecuteCache;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteFunc == null)
            {
                return true;
            }

            bool result = CanExecuteFunc(parameter);

            if (result != _canExecuteCache)
            {
                _canExecuteCache = result;
            }

            return result;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            if (ExecuteAction != null)
            {
                this.ExecuteAction(parameter);
            }
        }

        protected void FiredCanExecuteChanged()
        {
            //if (CanExecuteChanged != null)
            //    CanExecuteChanged(this, EventArgs.Empty);
        }

        public Action<object> ExecuteAction { get; set; }
        public Func<object, bool> CanExecuteFunc { get; set; }

    }
}
