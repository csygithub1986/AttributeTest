using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest
{
    /// <summary>
    /// 系统数据
    /// </summary>
    public class SystemData : ModelBase
    {
        private SystemData() { }
        private static SystemData _Instance;
        public static SystemData Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SystemData();
                }
                return _Instance;
            }
        }


        public Depot SystemDepot { get { return _SystemDepot; } set { if (_SystemDepot != value) { _SystemDepot = value; OnNotifyPropertyChanged("SystemDepot"); } } }
        private Depot _SystemDepot;


        public void Init()
        {
            var tracks = new ObservableCollection<Track>();
            Track track1 = new Track() { ID = "TrackID1", Name = "Track1" };
            Track track2 = new Track() { ID = "TrackID2", Name = "Track2" };
            tracks.Add(track1);
            tracks.Add(track2);

            Type ty = tracks.GetType().GetGenericTypeDefinition();


            var devices = new ObservableCollection<Device>();
            Device device1 = new Device() { ID = "DeviceID1", Name = "Device1", TrackBelong = track1 };
            Device device2 = new Device() { ID = "DeviceID2", Name = "Device2", TrackBelong = track2 };
            devices.Add(device1);
            devices.Add(device2);

            var doors = new ObservableCollection<Door>();
            Door door1 = new Door() { ID = "DoorID1", Name = "Door1", TrackBelong = track1 };
            Door door2 = new Door() { ID = "DoorID2", Name = "Door2", TrackBelong = track2 };
            doors.Add(door1);
            doors.Add(door2);

            var switchs = new ObservableCollection<Switch>();
            Switch switch1 = new Switch() { ID = "SwitchID1", Name = "Switch1", TrackBelong = track1 };
            Switch switch2 = new Switch() { ID = "SwitchID2", Name = "Switch2", TrackBelong = track2 };
            switchs.Add(switch1);
            switchs.Add(switch2);

            device1.DoorsRelated.Add(door1);
            device2.DoorsRelated.Add(door2);
            device1.SwitchsRelated.Add(switch1);
            device2.SwitchsRelated.Add(switch2);
            switch1.DevicesRelated.Add(device1);
            switch2.DevicesRelated.Add(device2);
            door1.DevicesRelated.Add(device1);
            door2.DevicesRelated.Add(device2);

            track1.Devices.Add(device1);
            track1.Doors.Add(door1);
            track1.Switchs.Add(switch1);
            track2.Devices.Add(device2);
            track2.Doors.Add(door2);
            track2.Switchs.Add(switch2);

            Depot depot = new Depot() { Devices = devices, Doors = doors, Switchs = switchs, Tracks = tracks };
            SystemDepot = depot;
        }
    }
}
