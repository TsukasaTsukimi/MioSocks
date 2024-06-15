using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class ModeBase
    {
        public string Type;
        public string Configure;
        private ServerBase Server;
        public ModeBase(ServerBase Server) { this.Server = Server; }
        public virtual void Start() { }
        public virtual void Stop() { }

    }
}
