using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    internal interface ServerInterface
    {
        string GetType();
        string Edit(string json);
        string AddServer();
        string Parse(string url);
        void Run(string json);
    }
}
