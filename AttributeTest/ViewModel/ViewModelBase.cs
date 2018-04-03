using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Private Event
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Public Method
        public void OnNotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
