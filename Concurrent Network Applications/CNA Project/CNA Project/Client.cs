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

namespace CNA_Project
{
    public class Client
    {
        TcpClient tcpClient;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;
        MainWindow form;

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
            form = new MainWindow(this);

            form.ShowDialog();

            while ((userInput = Console.ReadLine()) != null)
            {
                Thread thread = new Thread(() => { ProcessServerResponce(userInput); });
                thread.Start();
            }

            tcpClient.Close();
        }

        private void ProcessServerResponce(string message)
        {
            //Console.WriteLine("Server says: " + reader.ReadLine());
            //Console.WriteLine();
            while(tcpClient.Connected)
            {
                form.UpdateChatBox(message);
            }
        }

        public void SendMessage(string message)
        {
            Console.WriteLine("Server says: " + message);
            Console.WriteLine();
        }
    }
}
