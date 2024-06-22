using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace ModeNameSpace
{
    public class MioCoreMode : ModeBase
    {
        public override string Name { get { return "MioCoreMode"; } }
        private Process MioCore;
        public MioCoreMode(ServerBase Server) : base(Server) { }
        ~MioCoreMode() { }
        public override Process Start() 
        {
            Process process = Server.Start();
            MioCore = new Process();
            {
                MioCore.StartInfo = new ProcessStartInfo()
                {
                    FileName = "MioSocks-Core.exe",
                    Verb = "runas",
                };
            }
            MioCore.Start();
            return process;
        }
        public override void Stop() 
        {
            Server.Stop();
            if(!MioCore.HasExited)
            {
                MioCore.Kill();
            }
        }
    }
}
