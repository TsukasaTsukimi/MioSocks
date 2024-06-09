using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CommonLibrary;

namespace MioSocks_GUI
{
    public class ServerList : ObservableCollection<ServerBase>
	{
        public void Parse(string data)
        {
            var linklist = data.Split('\n').ToList();
            foreach (var link in linklist)
            {
                try
                {
                    ServerBase uri = new ServerBase(link);
                    this.Add(uri);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
    public class SubscriptionData
	{
		public bool Status { get; set; }
		public string Link { get; set; }

		public ServerList serverlist { get; set; }
    }
    public class SubscriptionList : ObservableCollection<SubscriptionData> { }

    public static class Subscription
    {
        static string filepath = "Subscription.json";
        public static SubscriptionList subscriptionlist = new SubscriptionList();
		public static ServerList serverlist = new ServerList();
		public static void Read()
		{
			try
			{
				using (StreamReader streamreader = new StreamReader(filepath))
				{
					string input = streamreader.ReadToEnd();
                    subscriptionlist = JsonConvert.DeserializeObject<SubscriptionList>(input);
				}
                foreach(var subscription in subscriptionlist)
				{
					if (subscription.serverlist != null) 
					{
                        foreach (var server in subscription.serverlist)
                        {
							serverlist.Add(server);
                        }
                    }
				}
            }
			catch(IOException e)
			{
				MessageBox.Show(e.Message);
			}
		}
		public static void Write()
		{
			string output = JsonConvert.SerializeObject(subscriptionlist, Formatting.Indented);
			try
			{
				using (StreamWriter streamwriter = new StreamWriter(filepath))
				{
					streamwriter.WriteLine(output);
				}
			}
			catch(IOException e)
			{
				MessageBox.Show(e.Message);
			}
		}

        public static void GetServer()
		{
			foreach (var subscription in subscriptionlist)
			{
				var website = subscription.Link;
				subscription.serverlist = new ServerList();
				try
				{
					WebRequest request = WebRequest.Create(website);
					WebResponse response = request.GetResponse();
					Stream datastream = response.GetResponseStream();
					StreamReader streamreader = new StreamReader(datastream);
					string server = streamreader.ReadToEnd();
					MessageBox.Show(server);
					string list = Base64.DecodeBase64(server);
					subscription.serverlist.Parse(list);
				}
				catch (IOException e)
				{
					MessageBox.Show(e.Message);
				}
			}
            Write();
        }
    }
}
