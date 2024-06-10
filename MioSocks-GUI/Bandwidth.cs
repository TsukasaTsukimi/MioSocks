using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System.Threading;
using System.Windows.Media;

namespace MioSocks_GUI
{
	public partial class MainWindow : Window
	{
		private readonly string[] Suffix = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };

        private string Compute(long d)
        {
            const double step = 1024.00;

            byte level = 0;
            double? size = null;
            while ((size ?? d) > step)
            {
                if (level >= 6) // Suffix.Length - 1
                    break;

                level++;
                size = (size ?? d) / step;
            }

            return $@"{size ?? 0:0.##} {Suffix[level]}";
        }

        public void NetTraffic(Process p)
		{
			long Received = 0;
			long Sent = 0;
            TraceEventSession EtwSession = null;
            Task.Run(() =>
			{
				EtwSession = new TraceEventSession("MyKernelAndClrEventsSession");

				EtwSession.EnableKernelProvider(KernelTraceEventParser.Keywords.NetworkTCPIP);

				EtwSession.Source.Kernel.TcpIpRecv += data =>
				{
					if (data.ProcessID == p.Id)
					{
						Received += data.size;
					}
				};

				EtwSession.Source.Kernel.TcpIpSend += data =>
				{
					if (data.ProcessID == p.Id)
					{
						Sent += data.size;
					}
				};

				EtwSession.Source.Process();
			});
			while (!p.HasExited)
			{
                Thread.Sleep(1000);
                Dispatcher.Invoke(() =>
				{
					this.General_Bandwidth_Label.Content = string.Format("Received:{0} Sent:{1}", Compute(Received), Compute(Sent));
				});
            }
            EtwSession?.Dispose();
            Dispatcher.Invoke(() =>
            {
                this.General_Bandwidth_Label.Content = "";
            });
        }
	}
}
