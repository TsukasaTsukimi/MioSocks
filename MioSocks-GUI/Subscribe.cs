using HandyControl.Controls;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

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
            string output = JsonConvert.SerializeObject(subscribelist);
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
    }
}
