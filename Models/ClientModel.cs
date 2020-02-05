using JClientBot.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JClientBot.Models
{
    public class ClientModel : JNotifier
    {
        public ClientModel(string name)
        {
            Name = name;
            clientViewList = new ClientViewInfoList();
        }
        private string name;
        private string recv;
        private string send;
        private string error;
        private string chat;
        private Point position;
        public string Name { get => name; set{ name = value; OnPropertyChanged("Name"); } }
        public string RecvMessage { get => recv; set { recv = value; OnPropertyChanged("RecvMessage"); } }
        public string SendMessage { get => send; set { send = value; OnPropertyChanged("SendMessage"); } }
        public string Error { get => error; set { error = value; OnPropertyChanged("Error"); } }
        public string Chat { get => chat; set { chat = value; OnPropertyChanged("Chat"); } }
        public Point Position { get => position; set { position = value; OnPropertyChanged("Position"); } }
        private ClientViewInfoList clientViewList;
        public ClientViewInfoList ViewList
        {
            get { return clientViewList; }
        }



        //소켓 통신
        private Socket socket;
        private byte[] data = new byte[2048];
        private int size = 2048;
        public void Connect(string _ID)
        {
            Socket newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9001);
            newSocket.BeginConnect(ipep, new AsyncCallback(Connected), newSocket);
        }

        public void Send(PACKET_HEADER packet)
        {
            if (socket == null)
                return;
            try
            {
                this.SendMessage = packet.command.ToString();
                byte[] message = PACKET_HEADER.Serialize(packet);
                socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(SendData), socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                this.Error = "socket closed";
            }
        }
        private void Connected(IAsyncResult result)
        {
            socket = (Socket)result.AsyncState;
            try
            {
                socket.EndConnect(result);
                PKS_CS_LOGIN loginPacket = new PKS_CS_LOGIN();
                loginPacket.command = PACKET_COMMAND.PACKET_CS_LOGIN;
                loginPacket.size = (uint)Marshal.SizeOf<PKS_CS_LOGIN>(loginPacket);
                Array.Copy(Name.ToCharArray(), loginPacket.commanderID, Name.Length);
                Send(loginPacket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                this.Error = "Connect Fail";
            }
        }
        private void ReceiveData(IAsyncResult result)
        {
            try
            {
                Socket remote = (Socket)result.AsyncState;
                int recvbyte = remote.EndReceive(result);
                if (recvbyte > 0)
                {
                    ProcessPacket();
                }
                remote.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), remote);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                this.Error = "read Fail";
            }

        }
        private void ProcessPacket()
        {
            var header = (PACKET_HEADER)PACKET_HEADER.Deserialize(data, typeof(PACKET_HEADER));

            this.RecvMessage = header.command.ToString();
            switch (header.command)
            {
                case PACKET_COMMAND.PACKET_SC_LOGIN_ACK:
                    {
                        var packet = (PKS_SC_LOGIN_ACK)PACKET_HEADER.Deserialize(data, typeof(PKS_SC_LOGIN_ACK));
                    }
                    break;
                case PACKET_COMMAND.PACKET_SC_CHAT:
                    {
                        var packet = (PKS_SC_CHAT)PACKET_HEADER.Deserialize(data, typeof(PKS_SC_CHAT));
                        this.Chat = new string(packet.chat).Split('\0')[0];
                    }
                    break;
                case PACKET_COMMAND.PACKET_SC_MOVE:
                    {
                        var packet = (PKS_SC_MOVE)PACKET_HEADER.Deserialize(data, typeof(PKS_SC_MOVE));
                        this.Position = new Point(packet.dest.X, packet.dest.Y);
                    }
                    break;
                case PACKET_COMMAND.PACKET_SC_VIEW:
                    {
                        var packet = (PKS_SC_VIEW)PACKET_HEADER.Deserialize(data, typeof(PKS_SC_VIEW));
                        if (ViewList == null)
                            break;
                        foreach (var view in ViewList)
                        {
                            view.IsUpdate = false;
                        }
                        var name = new string(packet.commanderID1).Split('\0')[0];
                        if (name != "None")
                        {
                            bool isFind = false;
                            foreach (var view in ViewList)
                            {
                                if (view.Name == name)
                                {
                                    view.Position = new Point(packet.dest1.X, packet.dest1.Y);
                                    view.IsUpdate = true;
                                    isFind = true;
                                    break;
                                }
                            }
                            if (!isFind)
                            {
                                var view = new ClientViewInfo(packet.dest1.X, packet.dest1.Y, name);
                                ViewList.Add(view);
                            }
                        }


                        name = new string(packet.commanderID2).Split('\0')[0];
                        if (name != "None")
                        {
                            bool isFind = false;
                            foreach (var view in ViewList)
                            {
                                if (view.Name == name)
                                {
                                    view.Position = new Point(packet.dest2.X, packet.dest2.Y);
                                    view.IsUpdate = true;
                                    isFind = true;
                                    break;
                                }
                            }
                            if (!isFind)
                            {
                                ViewList.Add(new ClientViewInfo(packet.dest2.X, packet.dest2.Y, name));
                            }
                        }
                        name = new string(packet.commanderID3).Split('\0')[0];
                        if (name != "None")
                        {
                            bool isFind = false;
                            foreach (var view in ViewList)
                            {
                                if (view.Name == name)
                                {
                                    view.Position = new Point(packet.dest3.X, packet.dest3.Y);
                                    view.IsUpdate = true;
                                    isFind = true;
                                    break;
                                }
                            }
                            if (!isFind)
                            {
                                ViewList.Add(new ClientViewInfo(packet.dest3.X, packet.dest3.Y, name));
                            }
                        }

                        name = new string(packet.commanderID4).Split('\0')[0];
                        if (name != "None")
                        {
                            bool isFind = false;
                            foreach (var view in ViewList)
                            {
                                if (view.Name == name)
                                {
                                    view.Position = new Point(packet.dest4.X, packet.dest4.Y);
                                    view.IsUpdate = true;
                                    isFind = true;
                                    break;
                                }
                            }
                            if (!isFind)
                            {
                                ViewList.Add(new ClientViewInfo(packet.dest4.X, packet.dest4.Y, name));
                            }
                        }

                        name = new string(packet.commanderID5).Split('\0')[0];
                        if (name != "None")
                        {
                            bool isFind = false;
                            foreach (var view in ViewList)
                            {
                                if (view.Name == name)
                                {
                                    view.Position = new Point(packet.dest5.X, packet.dest5.Y);
                                    view.IsUpdate = true;
                                    isFind = true;
                                    break;
                                }
                            }
                            if (!isFind)
                            {
                                ViewList.Add(new ClientViewInfo(packet.dest5.X, packet.dest5.Y, name));
                            }
                        }
                        foreach (var view in ViewList.ToList())
                        {
                            if (!view.IsUpdate)
                            {
                                ViewList.Remove(view);
                            }
                        }

                        /*

                        if (new string(packet.commanderID2).Split('\0')[0] == "")
                        {
                            break;
                        }
                        clientViewList.Add(new ClientView(packet.dest2.X, packet.dest2.Y, new string(packet.commanderID2).Split('\0')[0]));


                        if (new string(packet.commanderID3).Split('\0')[0] == "")
                        {
                            break;
                        }
                        clientViewList.Add(new ClientView(packet.dest3.X, packet.dest3.Y, new string(packet.commanderID3).Split('\0')[0]));
                        */


                    }
                    break;

                default:
                    this.Error = "wrong packet command";
                    break;
            }
        }
        private void SendData(IAsyncResult iar)
        {
            try
            {
                Socket remote = (Socket)iar.AsyncState;
                int sent = remote.EndSend(iar);
                remote.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), remote);
            }
            catch
            { }
        }
        public void Close()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), socket);
        }

        private static void DisconnectCallback(IAsyncResult ar)
        {
            // Complete the disconnect request.
            Socket client = (Socket)ar.AsyncState;
            client.EndDisconnect(ar);
        }
    }

}
