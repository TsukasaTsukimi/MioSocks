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
using ServerNameSpace;
using ModeNameSpace;

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
        static ModeBase modebase;
        private void General_Start_Button_Click(object sender, RoutedEventArgs e)
        {
			try
			{
                ServerBase serverBase = new ServerData((ServerBase)General_Server_ComboBox.SelectedItem);
                modebase = new ModeData(serverBase);
                Process p = modebase.Start();

                TabWindow_Add(new List<Process> { p });
                Task.Run(() =>
                {
                    NetTraffic(p);
                });
            }
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
            General_Start_Button.Visibility = Visibility.Collapsed;
            General_Stop_button.Visibility = Visibility.Visible;
        }

        private void General_Stop_button_Click(object sender, RoutedEventArgs e)
        {
            General_TabControl.Items.Clear();
            modebase.Stop();
            General_Stop_button.Visibility = Visibility.Collapsed;
            General_Start_Button.Visibility = Visibility.Visible;
        }

        private void General_Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (General_Server_ComboBox.SelectedIndex == -1)
                return;
            ServerNameSpace.ServerWindow a = new ServerNameSpace.ServerWindow((ServerBase)General_Server_ComboBox.SelectedItem);
            a.ShowDialog();
        }

    }
}
