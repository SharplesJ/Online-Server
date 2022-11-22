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

namespace SeverProj
{
    public class Server
    {
        private TcpListener m_tcpListener;
        ConcurrentDictionary<int, ConnectedClient> Clients;
        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_tcpListener = new TcpListener(ip, port);
        }
        public void Start()
        {
            Clients = new ConcurrentDictionary<int, ConnectedClient>();
            int clientIndex = 0;
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
            string receivedMessage;

            //Clients[index].stream = new NetworkStream(socket, true);
            while ((receivedMessage = Clients[index].Read()) != null)
            {
                Clients[index].Send(receivedMessage);
            }
            Clients[index].Close();
            ConnectedClient c;
            Clients.TryRemove(index, out c);
        }
        //private void ClientMethod(Socket socket)
        //{
        //    string receivedMessage;

        //    NetworkStream stream = new NetworkStream(socket, true);
        //    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        //    StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

        //    writer.WriteLine("You have connecter to the server - send 0 for valid options");
        //    writer.Flush();

        //    while ((receivedMessage = reader.ReadLine()) != null)
        //    {
        //        writer.WriteLine(GetReturnMessage(receivedMessage));
        //        writer.Flush();

        //        if (receivedMessage == "bye")
        //            break;
        //    }

        //    socket.Close();
        //}

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
