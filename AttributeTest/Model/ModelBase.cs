using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest
{
    public class ModelBase : INotifyPropertyChanged
    {
        #region 保护函数
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 引发 PropertyChanged 事件。
        /// </summary>
        /// <param name="e">事件数据。</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        /// <summary>
        /// 属性更改事件
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnNotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
