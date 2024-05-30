using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;

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
		public static SubscribeList subscribelist = new SubscribeList();
		static string filepath = "Subscribe.json";
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
				string list = DecodeBase64(server);
				using(StreamWriter streamwriter = new StreamWriter("test.txt"))
				{
					streamwriter.Write(list);
				}
            }
			catch(IOException e)
			{
				MessageBox.Show(e.Message);
			}
		}

        public static string DecodeBase64(string val)
        {
            return Encoding.UTF8.GetString(DecodeBase64ToBytes(val));
        }

        public static byte[] DecodeBase64ToBytes(string val)
        {
            var data = val.PadRight(val.Length + (4 - val.Length % 4) % 4, '=');
            return Convert.FromBase64String(data);
        }
    }
}
