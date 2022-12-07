using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Packets;

namespace SeverProj
{
    internal class ConnectedClient
    {
        Socket socket;
        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;
        BinaryFormatter formatter;
        public string name;


        object readLock;
        object writeLock;

        int numberOfBytes;



        public ConnectedClient(Socket socket)
        {
            writeLock = new object();
            readLock = new object();

            stream = new NetworkStream(socket, true);
            writer = new BinaryWriter(stream, Encoding.UTF8);
            reader = new BinaryReader(stream, Encoding.UTF8);
            formatter = new BinaryFormatter();
        }



        public void Close()
        {
            stream.Close();
            reader.Close();
            writer.Close();
            socket.Close();
        }
        public Packet Read()
        {
            if((numberOfBytes = reader.ReadInt32()) != -1)
            {
                byte[] buffer = reader.ReadBytes(numberOfBytes);
                MemoryStream ms = new MemoryStream(buffer);
                return formatter.Deserialize(ms) as Packet;
            }
            else
                return null;
        }
        public void Send(Packet packet)
        {
            lock (writeLock)
            {
                MemoryStream ms = new MemoryStream();
                formatter.Serialize(ms, packet);
                byte[] buffer = ms.GetBuffer();
                writer.Write(buffer.Length);
                writer.Write(buffer);
                writer.Flush();
            }
        }

        public void Send(Packet packet, int index)
        {
            lock (writeLock)
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

}

