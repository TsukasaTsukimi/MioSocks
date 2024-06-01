using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MioSocks_GUI
{
	public class SubscribeData
	{
		public bool Status { get; set; }
		public string Link { get; set; }
	}

	public class SubscribeList : ObservableCollection<SubscribeData> { }

	public static class Subscribe
	{
        static string filepath = "Subscribe.json";
        public static SubscribeList subscribelist = new SubscribeList();
		public static void Read()
		{
			try
			{
				using (StreamReader streamreader = new StreamReader(filepath))
				{
					string input = streamreader.ReadToEnd();
					subscribelist = JsonConvert.DeserializeObject<SubscribeList>(input);
				}
			}
			catch/*(IOException e)*/
			{
			}
		}
		public static void Write()
		{
			string output = JsonConvert.SerializeObject(subscribelist, Formatting.Indented);
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
			var website = subscribelist[0].Link;
			try
			{
                WebRequest request = WebRequest.Create(website);
                WebResponse response = request.GetResponse();
                Stream datastream = response.GetResponseStream();
                StreamReader streamreader = new StreamReader(datastream);
				string server = streamreader.ReadToEnd();
				MessageBox.Show(server);
				string list = Base64.DecodeBase64(server);
				Server.Parse(list);
            }
			catch(IOException e)
			{
				MessageBox.Show(e.Message);
			}
		}
    }
}
