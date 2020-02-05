using JClientBot.Commons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JClientBot
{
    public class ClientViewInfoList : ObservableCollection<ClientViewInfo> { }
    public class ClientViewInfo : JNotifier
    {
        public ClientViewInfo(float positionX, float positionY, string name)
        {
            position.X = positionX;
            position.Y = positionY;
            IsUpdate = true;
            Name = name;
            OnPropertyChanged("Position");
        }
        private Point position;
        public Point Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }
        
        public string Name { get; set; }
        public bool IsUpdate { get; set; }
    }
}
