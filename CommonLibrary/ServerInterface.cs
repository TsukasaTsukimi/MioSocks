using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CommonLibrary
{
	public class ServerBase
	{
		public string Scheme { get; set; }
        public string UserInfo { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
		public string Query { get; set; }
        public string Fragment { get; set; }
        [JsonIgnore]
		public string title { get => string.Format($"[{Scheme}]{Fragment}"); }
        [JsonIgnore]
        public string uri { get => string.Format($"{Scheme}://{UserInfo}@{Host}:{Port}/{Query}{Fragment}"); }

        public ServerBase() { }

        public ServerBase(ServerBase data) 
		{
			Scheme = data.Scheme;
			UserInfo = data.UserInfo;
			Host = data.Host;
			Port = data.Port;
			Query = data.Query;
			Fragment = data.Fragment;
		}
        public ServerBase(string uriString)
		{
			Uri uri = new Uri(uriString);
			Scheme = uri.Scheme;
			Fragment = Uri.UnescapeDataString(uri.Fragment);
			Host = uri.Host;
			Port = uri.Port;
			UserInfo = uri.UserInfo;
			Query = uri.Query;
		}
		public virtual Process Start() { return null; }
		public virtual void Stop() { }
	}
}
