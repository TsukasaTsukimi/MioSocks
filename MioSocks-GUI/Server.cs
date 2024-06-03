using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyControl.Controls;
using System.Windows.Documents;
using CommonLibrary;

namespace MioSocks_GUI
{
	
	public class ServerList : ObservableCollection<ServerBase> { }
	public static class Server
	{
		static string filepath = "Server.json";
		public static ServerList serverlist = new ServerList();
        public static void Read()
		{
			try
			{
				using (StreamReader streamreader = new StreamReader(filepath))
				{
					string data = streamreader.ReadToEnd();
					serverlist = JsonConvert.DeserializeObject<ServerList>(data);
                }
			}
			catch /*(Exception e)*/
			{
            }
        }
		public static void Write()
		{
			string output = JsonConvert.SerializeObject(serverlist, Formatting.Indented);
			try
			{
				using (StreamWriter streamwriter = new StreamWriter(filepath))
				{
					streamwriter.WriteLine(output);
				}
			}
			catch (IOException e)
			{
				MessageBox.Show(e.Message);
			}
		}
		public static void Parse(string data)
		{
			var linklist = data.Split('\n').ToList();
			foreach (var link in linklist)
			{
                try
				{
                    ServerBase uri = new ServerBase(link);
                    serverlist.Add(uri);
                }
				catch(Exception e) 
				{
					MessageBox.Show(e.Message);
				}
			}
			Write();
		}
	}
}
