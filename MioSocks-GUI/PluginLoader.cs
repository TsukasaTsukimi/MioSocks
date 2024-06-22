using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MioSocks_GUI
{
    public class PluginLoader
    {
        public Dictionary<string, Type> Dict;
        private const string ServerPath = @".\plugin\server\";
        private const string ModePath = @".\plugin\mode\";
        public PluginLoader()
        {
            Dict = new Dictionary<string, Type>();
            {
                string[] dllFiles = Directory.GetFiles(ModePath, "*.dll");
                foreach (string dllFile in dllFiles)
                {
                    string filename = Path.GetFileNameWithoutExtension(dllFile);
                    Assembly assembly = Assembly.LoadFrom(dllFile);
                    string ServerClassName = "ModeNameSpace.ModeData";
                    Type type = assembly.GetType(ServerClassName);
                    if (type != null)
                    {
                        Dict.Add(filename, type);
                    }
                }
            }
            {
                string[] dllFiles = Directory.GetFiles(ServerPath, "*.dll");
                foreach (string dllFile in dllFiles)
                {
                    string filename = Path.GetFileNameWithoutExtension(dllFile);
                    Assembly assembly = Assembly.LoadFrom(dllFile);
                    string ServerClassName = "ServerNameSpace.ServerData";
                    Type type = assembly.GetType(ServerClassName);
                    if (type != null)
                    {
                        Dict.Add(filename, type);
                    }
                }
            }
        }
    }
}
