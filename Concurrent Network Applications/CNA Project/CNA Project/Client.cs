using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Net;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using Packets;

namespace CNA_Project
{
    public class Client
    {
        TcpClient tcpClient;
        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;
        MainWindow form;
        BinaryFormatter formatter;

        public Client()
        {
            tcpClient = new TcpClient();
            formatter = new BinaryFormatter();
        }
        public bool Connect(string ipAdress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAdress);
            try
            {
                tcpClient.Connect(ip, port);
                stream = tcpClient.GetStream();
                writer = new BinaryWriter(stream);
                reader = new BinaryReader(stream);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }
        }

        public void Run()
        {
            form = new MainWindow(this);
            Thread thread = new Thread(() => { ProcessServerResponce(); });
            thread.Start();


            form.ShowDialog();

            tcpClient.Close();
        }

        private void ProcessServerResponce()
        {
            int numberOfBytes;
            while ((numberOfBytes = reader.ReadInt32()) != -1)
            {
                byte[] buffer = reader.ReadBytes(numberOfBytes);
                MemoryStream ms = new MemoryStream(buffer);
                Packet receivedMessage = formatter.Deserialize(ms) as Packet;
                if (receivedMessage != null)
                {
                    switch (receivedMessage.m_PacketType)
                    {
                        case Packets.PacketType.CLIENT_NAME:
                            ClientNamePacket clientNamePacket = (ClientNamePacket)receivedMessage;
                            
                            form.UpdateChatBox(clientNamePacket.m_LocalName + " says: ");
                            break;
                        case Packets.PacketType.CHAT_MESSAGE:
                            ChatMessagePacket chatPacket = (ChatMessagePacket)receivedMessage;

                            form.UpdateChatBox(chatPacket.m_Message + "\n");
                            break;
                        case Packets.PacketType.PRIVATE_MESSAGE:
                            PrivateNamePacket privateNamePacket = (PrivateNamePacket)receivedMessage;

                            form.UpdateChatBox(privateNamePacket.m_LocalName + " says: '" + privateNamePacket.m_Message + "' to you" + "\n");
                            break;
                    }
                }
            }
        }

        public void SendMessage(Packet packet)
        {
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();
            writer.Write(buffer.Length);
            writer.Write(buffer);
            writer.Flush();
        }
    }
}
