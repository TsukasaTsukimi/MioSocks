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
using CommonLibrary;

namespace ServerNameSpace
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ServerWindow : Window
    {
        public ServerWindow()
        {
            InitializeComponent();
        }

        public ServerWindow(ServerBase basedata)
        {
            InitializeComponent();
            ServerData data = new ServerData(basedata);
            ServerIP_PasswordBox.Password = data.Host;
            Port_NumericUpDown.Text = data.Port.ToString();
            Password_PasswordBox.Password = data.password;
            Encryption_ComboBox.Text = data.method;
            Remark_TextBox.Text = data.Fragment;
            SS_Link_TextBox.Text = data.uri;
        }

        private void ServerIP_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ServerIP_PasswordBox.Visibility= Visibility.Collapsed;
            ServerIP_TextBox.Visibility = Visibility.Visible;
            ServerIP_TextBox.Text = ServerIP_PasswordBox.Password;
        }

        private void ServerIP_CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            ServerIP_TextBox.Visibility = Visibility.Collapsed;
            ServerIP_PasswordBox.Visibility = Visibility.Visible;
            ServerIP_PasswordBox.Password = ServerIP_TextBox.Text;
        }
        private void Password_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Password_PasswordBox.Visibility = Visibility.Collapsed;
            Password_TextBox.Visibility = Visibility.Visible;
            Password_TextBox.Text = Password_PasswordBox.Password;
        }

        private void Password_CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            Password_TextBox.Visibility = Visibility.Collapsed;
            Password_PasswordBox.Visibility = Visibility.Visible;
            Password_PasswordBox.Password = Password_TextBox.Text;
        }
    }
}
