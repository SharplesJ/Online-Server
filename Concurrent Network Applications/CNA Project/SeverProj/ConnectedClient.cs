using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;


namespace SeverProj
{
    internal class ConnectedClient
    {
        Socket socket;
        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;
        object readLock;
        object writeLock;



        public ConnectedClient(Socket socket)
        {
            writeLock = new object();
            readLock = new object();
            stream = new NetworkStream(socket, true);
            writer = new StreamWriter(stream, Encoding.UTF8);
            reader = new StreamReader(stream, Encoding.UTF8);
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
        public void Send(string message)
        {
            lock (writeLock)
            {
                writer.Write(message);
                writer.Flush();
            }
        }
    }

}

