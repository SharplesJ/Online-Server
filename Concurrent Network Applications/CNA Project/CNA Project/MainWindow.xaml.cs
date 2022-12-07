using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Packets;

namespace CNA_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ownName = "";
        Client m_Client;

        public MainWindow(Client client)
        {
            InitializeComponent();
            m_Client = client;
        }

        public void UpdateChatBox(string message)
        {
            chatBox.Dispatcher.Invoke(() =>
            {
                chatBox.Text += message;
                chatBox.ScrollToEnd();
            });
        }

        //public void UpdateClientBox(string message)
        //{
        //    connectedClients.Dispatcher.Invoke(() =>
        //    {
                
        //        connectedClients.Text += message;
        //        connectedClients.ScrollToEnd();
        //    });
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string message = messageText.Text;
            string name = localName.Text;
            string target = targetName.Text;

            if (messageText.Text == "")
            {
                MessageBox.Show("No message in text box!", "Warning");
            }
            else
            {
                if (localName.Text == "")
                {
                    MessageBox.Show("Please enter a name in Local Name Textbox!", "Warning");
                    messageText.Text = message;
                }
                else if (@private.IsChecked == true)
                {

                    m_Client.SendMessage(new PrivateNamePacket(message, target, name));

                    messageText.Text = "";
                    chatBox.Text += "You Whisper '" + message + "' to client " + target + "\n";
                }
                else
                {
                    m_Client.SendMessage(new ClientNamePacket(name));
                    m_Client.SendMessage(new ChatMessagePacket(message));

                    messageText.Text = "";
                    chatBox.Text += name + " says: " + message + "\n";
                }
            }

            if(ownName != name)
            {
                if (ownName != "")
                    connectedClients.Text += ownName + " left- \n";
                connectedClients.Text += name + " joined+ \n";
                ownName = name;
            }

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
