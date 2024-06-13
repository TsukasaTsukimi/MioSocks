using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.ComponentModel;
using Shadowsocks;
using System.IO;
using System.Threading;

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

        }
        ~MainWindow()
        {
            foreach(TabWindowItem item in General_TabControl.Items)
            {
                item.process.Kill();
            }
            foreach(Process p in Process.GetProcessesByName("simple-obfs"))
            {
                p.Kill();
            }
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

        static Process Proxy;
        static Process MioCore;
        private void General_Start_Button_Click(object sender, RoutedEventArgs e)
        {
			try
			{
                Proxy = new Process();
                {
                    var item = (ServerBase)General_Server_ComboBox.SelectedItem;
                    Proxy.StartInfo = new ProcessStartInfo()
                    {
                        FileName = "sslocal.exe",
                        Arguments = String.Format("-b 127.0.0.1:2801 --server-url \"{0}\"", item.uri),
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };
                }
                
                MioCore = new Process();
                {
                    MioCore.StartInfo = new ProcessStartInfo()
                    {
                        FileName = "MioSocks-Core.exe",
                        Verb = "runas",
                    };
                }
                Bandwidth_Add(new List<Process> { Proxy});
                Task.Run(() =>
                {
                    NetTraffic(MioCore);
                });
                MioCore.Start();

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
