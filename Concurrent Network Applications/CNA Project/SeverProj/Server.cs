using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace SeverProj
{
    internal class Server
    {
        private TcpListener m_tcpListener;
        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_tcpListener = new TcpListener(ip, port);
        }
        public void Start()
        {
            m_tcpListener.Start();

            Console.WriteLine("Listening...");

            Socket socket = m_tcpListener.AcceptSocket();
            Console.WriteLine("Connection Made");
            ClientMethod(socket);
        }
        public void Stop()
        {
            m_tcpListener.Stop();
        }

        private void ClientMethod(Socket socket)
        {
            string receivedMessage;

            NetworkStream stream = new NetworkStream(socket, true);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            writer.WriteLine("You have connecter to the server - send 0 for valid options");
            writer.Flush();

            while ((receivedMessage = reader.ReadLine()) != null)
            {
                writer.WriteLine(GetReturnMessage(receivedMessage));
                writer.Flush();

                if (receivedMessage == "bye")
                    break;
            }

            socket.Close();
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
