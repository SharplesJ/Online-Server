using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace SeverProj
{
    internal class ConnectedClient
    {
        Socket socket;
        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;
        BinaryFormatter formatter;
        object readLock;
        object writeLock;



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
        public string Read()
        {
            lock (readLock)
            {
                return reader.ReadLine();
            }
        }

        public void Send(Packets packet)
        {
            lock (writeLock)
            {
                MemoryStream memStream = new MemoryStream();
                formatter.Serialize(memStream, packet);
                byte[] buffer = memStream.GetBuffer();
                writer.Write(buffer.Length);
                writer.Write(buffer);
                writer.Flush();
            }
        }
    }

}

