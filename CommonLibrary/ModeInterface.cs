using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class ModeBase
    {
        public virtual string Name { get; }
        public virtual string Configure { get; set; }
        protected ServerBase Server;
        public ModeBase(ServerBase Server) { this.Server = Server; }
        public virtual Process Start() { return null; }
        public virtual void Stop() { }

    }
}
