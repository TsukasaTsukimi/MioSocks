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
		}

		private void Subscription_Add_Click(object sender, RoutedEventArgs e)
		{
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
    }
}
