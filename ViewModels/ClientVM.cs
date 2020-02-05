using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Linq;
using System.Windows;
using JClientBot.Models;
using JClientBot.Commons;
using System.Windows.Input;

namespace JClientBot
{
    public class ClientList : ObservableCollection<ClientModel> { }
    public class ClientVM : JNotifier
    {
        public ClientVM()
        {
            CreateClients = new Command(CreateClientFunc, CanExecuteMethod);
            MoveRandom = new Command(MoveClientRandomFunc, CanExecuteMethod);
            LogoutClients = new Command(LogoutClientsFunc, CanExecuteMethod);
            SelectClient = new Command(SelectClientFunc, CanExecuteMethod);
            CreateClientName = "Test";
            CreateClientCount = 50;
            _clientList = new ClientList();
        }

        public string CreateClientName { get; set; }
        public int CreateClientCount { get; set; }
        private ClientList _clientList;
        public ClientList ClientList
        {
            get
            {
                return _clientList;
            }
        }
        private ClientViewInfoList viewInfoList;
        public ClientViewInfoList ViewInfoList { get => viewInfoList; set { viewInfoList = value; OnPropertyChanged("ViewInfoList"); } }

        public ICommand CreateClients { get; set; }
        public ICommand MoveRandom { get; set; }
        public ICommand LogoutClients { get; set; }
        public ICommand SelectClient { get; set; }

        private bool CanExecuteMethod(object arg)
        {
            return true;
        }
        private void CreateClientFunc(object obj)
        {
            ClientList.Clear();
            int reqClientCount = CreateClientCount;
            for (int i = 0; i < reqClientCount; i++)
            {
                ClientModel client = new ClientModel(CreateClientName + i.ToString());
                //client.Name = ID_Text.Text + i.ToString();
                client.Connect("");
                client.Error = "-";
                client.Chat = "1";
                ClientList.Add(client);
            }
        }
        private void MoveClientRandomFunc(object obj)
        {
            int clientCount = ClientList.Count();
            Random r = new Random();
            foreach (var client in ClientList)
            {
                PKS_CS_MOVE packet = new PKS_CS_MOVE();
                packet.command = PACKET_COMMAND.PACKET_CS_MOVE;
                packet.size = (uint)Marshal.SizeOf<PKS_CS_MOVE>();
                packet.dest = new Vector3(r.Next(0, 300), r.Next(0, 300), r.Next(0, 300));
                client.Send(packet);
            }
        }
        private void LogoutClientsFunc(object obj)
        {
            foreach (var client in ClientList)
            {
                PKS_CS_LOGOUT packet = new PKS_CS_LOGOUT();
                packet.command = PACKET_COMMAND.PACKET_CS_LOGOUT;
                packet.size = (uint)Marshal.SizeOf<PKS_CS_LOGOUT>();
                client.Send(packet);
                client.Close();
            }
        }
        private void SelectClientFunc(object obj)
        {
            //임시로 아무나 선택
            Random random = new Random();
            var client = ClientList[random.Next(0, ClientList.Count - 1)];
            ViewInfoList = client.ViewList;
        }
    }
}
