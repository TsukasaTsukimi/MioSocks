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
            Subscribe.Read();
            Subscription_DataGrid.ItemsSource = Subscribe.subscribelist;

			Server.Read();
			General_Server_ComboBox.ItemsSource = Server.serverlist;
			General_Server_ComboBox.DisplayMemberPath = "title";

        }

        private void Subscription_Add_Click(object sender, RoutedEventArgs e)
		{
			if (Subscription_Link_TextBox.Text == "")
				return;
            Subscribe.subscribelist.Add(
				new SubscribeData
				{
					Status = true,
					Link = Subscription_Link_TextBox.Text
				}
			);
			Subscribe.Write();
			Subscription_Link_TextBox.Text = "";
        }

		private void Subscription_Delete_Click(object sender, RoutedEventArgs e)
		{
			if (Subscription_DataGrid.SelectedItems.Count == 0)
				return;
			var selectedItems = Subscription_DataGrid.SelectedItems.Cast<SubscribeData>().ToList();
			foreach(SubscribeData sub in selectedItems) 
			{
                Subscribe.subscribelist.Remove(sub);
            }
            Subscribe.Write();
        }

        private void SubScription_Refresh_Click(object sender, RoutedEventArgs e)
        {
			Subscribe.GetServer();
        }

        private void General_Start_Button_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				using(Process p = new Process())
				{
					var proxy = (ServerBase)General_Server_ComboBox.SelectedItem;
                    p.StartInfo.FileName = "sslocal.exe";
					p.StartInfo.Arguments = String.Format("-b 127.0.0.1:2806 --server-url \"{0}\"", proxy.uri);
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
