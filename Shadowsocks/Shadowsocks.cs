﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace ServerNameSpace
{
	public class ServerData: ServerBase
	{
		public ServerData(ServerBase data) : base(data) { }
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
	}
}