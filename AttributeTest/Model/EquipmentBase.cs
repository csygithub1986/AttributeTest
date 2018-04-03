using AttributeTest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest
{
    public class EquipmentBase : ModelBase
    {
        public string ID { get { return _ID; } set { if (_ID != value) { _ID = value; OnNotifyPropertyChanged("ID"); } } }
        private string _ID;

        public string Name { get { return _Name; } set { if (_Name != value) { _Name = value; OnNotifyPropertyChanged("Name"); } } }
        private string _Name;

        [OperationConfigMark("合闸")]
        [OperationCondition]
        public bool IsClosed { get { return _IsClosed; } set { if (_IsClosed != value) { _IsClosed = value; OnNotifyPropertyChanged("IsClosed"); } } }
        private bool _IsClosed;

        [OperationConfigMark("股道")]
        public Track TrackBelong { get { return _TrackBelong; } set { if (_TrackBelong != value) { _TrackBelong = value; OnNotifyPropertyChanged("TrackBelong"); } } }
        private Track _TrackBelong;
    }
}
