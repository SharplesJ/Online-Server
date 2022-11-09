﻿using System;
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

namespace CNA_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (messageText.Text == "")
            {
                MessageBox.Show("No message in text box!", "Warning");
            }
            else
            {
                string message = messageText.Text;
                messageText.Text = "";
                if (localName.Text == "")
                {
                    MessageBox.Show("Please enter a name in Local Name Textbox!", "Warning");
                    messageText.Text = message;
                }
                else
                {
                    string name = localName.Text;
                    chatBox.Text += name + " says: " + message + "\n";
                }
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
