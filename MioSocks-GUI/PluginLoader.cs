using CommonLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MioSocks_GUI
{
    public class PluginLoader
    {
        private Dictionary<string, Assembly> Dict;
        private const string ServerPath = @".\plugin\server\";
        private const string ModePath = @".\plugin\mode\";
        public PluginLoader()
        {
            Dict = new Dictionary<string, Assembly>();
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
                        Dict.Add(filename, assembly);
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
                        Dict.Add(filename, assembly);
                    }
                }
            }
        }
        public ServerBase CreateServerInstance(ServerBase serverbase)
        {
            try
            {
                Assembly assembly = Dict[serverbase.Scheme];
                Type serverclass = assembly.GetType("ServerNameSpace.ServerData");
                ServerBase serverdata = (ServerBase)Activator.CreateInstance(serverclass, serverbase);
                return serverdata;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        public Window CreateWindowInstance(ServerBase serverbase)
        {
            try
            {
                Assembly assembly = Dict[serverbase.Scheme];
                Type windowclass = assembly.GetType("ServerNameSpace.ServerWindow");
                Window window = (Window)Activator.CreateInstance(windowclass, serverbase);
                return window;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
