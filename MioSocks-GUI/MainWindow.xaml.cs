using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Diagnostics;
using System.ComponentModel;
using Shadowsocks;
using HandyControl.Tools.Extension;
using System.IO;

namespace MioSocks_GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        public MainWindow()
		{
			InitializeComponent();

			/* Read Subscription List from Json File */
            Subscription.Read();

			Subscription_DataGrid.ItemsSource = Subscription.subscriptionlist;

            General_Server_ComboBox.ItemsSource = Subscription.serverlist;
            General_Server_ComboBox.DisplayMemberPath = "title";

            /*Server.Read();
			General_Server_ComboBox.ItemsSource = Server.serverlist;
			General_Server_ComboBox.DisplayMemberPath = "title";*/

        }

        private void Subscription_Add_Click(object sender, RoutedEventArgs e)
		{
			if (Subscription_Link_TextBox.Text == "")
				return;
            Subscription.subscriptionlist.Add(
				new SubscriptionData
				{
					Status = true,
					Link = Subscription_Link_TextBox.Text
				}
			);
            Subscription.Write();
			Subscription_Link_TextBox.Text = "";
        }

		private void Subscription_Delete_Click(object sender, RoutedEventArgs e)
		{
			if (Subscription_DataGrid.SelectedItems.Count == 0)
				return;
			var selectedItems = Subscription_DataGrid.SelectedItems.Cast<SubscriptionData>().ToList();
			foreach(SubscriptionData sub in selectedItems) 
			{
                Subscription.subscriptionlist.Remove(sub);
            }
            Subscription.Write();
        }

        private void SubScription_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Subscription.GetServer();
        }

        private void General_Start_Button_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				using(Process p = new Process())
				{
					var item = (ServerBase)General_Server_ComboBox.SelectedItem;
                    p.StartInfo.FileName = "sslocal.exe";
					p.StartInfo.Arguments = String.Format("-b 127.0.0.1:2801 --server-url \"{0}\"", item.uri);
					p.Start();
                }
				System.Threading.Thread.Sleep(3000);
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = "MioSocks-Core.exe";
					p.StartInfo.Verb = "runas";
                    p.Start();
                }
            }
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
        }

        private void General_Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            ServerNameSpace.ServerWindow a = new ServerNameSpace.ServerWindow((ServerBase)General_Server_ComboBox.SelectedItem);
            a.ShowDialog();
        }
    }
}
