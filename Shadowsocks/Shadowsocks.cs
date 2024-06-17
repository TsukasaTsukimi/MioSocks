using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace ServerNameSpace
{
	public class ServerData: ServerBase
	{
		public ServerData(ServerBase data) : base(data) { }
        ~ServerData(){ Stop(); }
		private string[] UserInfoList 
		{
			get
			{
				string decode = Base64.DecodeBase64(UserInfo);
				return decode.Split(':');
			}
		}
		public string method { get => UserInfoList[0]; }
		public string password { get => UserInfoList[1]; }
		public UriQueryDict dictionary { get => new UriQueryDict(Query); }
        private Process shadowsocks;
        public override Process Start()
        {
            shadowsocks = new Process()
            {
                StartInfo =
                {
                    FileName = "sslocal.exe",
                    Arguments = String.Format("-b 127.0.0.1:2801 --server-url \"{0}\"", uri),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            return shadowsocks;
        }
        public override void Stop()
        {
            if (shadowsocks != null)
            {
                foreach (Process p in Process.GetProcessesByName("simple-obfs"))
                {
                    p.Kill();
                }
                if (!shadowsocks.HasExited)
                {
                    shadowsocks.Kill();
                }
            }
        }
    }
}
