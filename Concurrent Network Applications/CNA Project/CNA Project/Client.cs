using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace CNA_Project
{
    class Client
    {
        TcpClient tcpClient;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;

        public Client()
        {
            tcpClient = new TcpClient();
        }
        public bool Connect(string ipAdress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAdress);
            try
            {
                tcpClient.Connect(ip, port);
                stream = tcpClient.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);
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
            string userInput;
            ProcessServerResponce();
            while((userInput = Console.ReadLine()) != null)
            {
                writer.WriteLine(userInput);
                writer.Flush();

                ProcessServerResponce();

                if (userInput == "bye")
                    break;
            }

            tcpClient.Close();
        }

        private void ProcessServerResponce()
        {
            Console.WriteLine("Server says: " + reader.ReadLine());
            Console.WriteLine();
        }
    }
}
