using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using Packets;

namespace SeverProj
{
    public class Server
    {
        private TcpListener m_tcpListener;
        ConcurrentDictionary<int, ConnectedClient> Clients;
        List<string> clientlist = new List<string>();
        int clientIndex = 0;

        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_tcpListener = new TcpListener(ip, port);
        }
        public void Start()
        {
            Clients = new ConcurrentDictionary<int, ConnectedClient>();
            m_tcpListener.Start();

            while(true)
            {
                Console.WriteLine("Listening...");

                Socket socket = m_tcpListener.AcceptSocket();
                ConnectedClient m_Clients = new ConnectedClient(socket);

                int index = clientIndex;
                clientIndex++;

                Thread thread = new Thread(() => { ClientMethod(index); });
                thread.Start();

                Clients.TryAdd(index, m_Clients);

                Console.WriteLine("Connection Made");
                //ClientMethod(socket);

            }
        }
        public void Stop()
        {
            m_tcpListener.Stop();
        }

        private void ClientMethod(int index)
        {
            Packet receivedMessage;

            //Clients[index].stream = new NetworkStream(socket, true);
            while ((receivedMessage = Clients[index].Read()) != null)
            {
                if(receivedMessage != null)
                {
                    switch (receivedMessage.m_PacketType)
                    {
                        case Packets.PacketType.CLIENT_NAME:
                            ClientNamePacket clientNamePacket = (ClientNamePacket)receivedMessage;
                            for (int i = 0; i < clientIndex; i++)
                            {
                                if (i != index)
                                    Clients[i].Send(receivedMessage);
                            }
                            break;
                        case Packets.PacketType.CHAT_MESSAGE:
                            //ChatMessagePacket chatPacket = (ChatMessagePacket)receivedMessage;
                            for (int i = 0; i < clientIndex; i++)
                            {
                                if (i != index)
                                    Clients[i].Send(receivedMessage);
                            }
                            break;
                        case Packets.PacketType.PRIVATE_MESSAGE:
                            PrivateNamePacket privateNamePacket = (PrivateNamePacket)receivedMessage;

                            Clients[Int32.Parse(privateNamePacket.m_target)].Send(receivedMessage);
                            break;
                    }
                }
            }
            Clients[index].Close();
            ConnectedClient c;
            Clients.TryRemove(index, out c);
        }

        private string GetReturnMessage(string code)
        {
            if(code == "Hi")
            {
                return "Hello";
            }
            else if(code == "bye")
            {
                return "Goodbye Friend";
            }
            else
            {
                return "What an interesting thing to say";
            }
        }


    }
}
