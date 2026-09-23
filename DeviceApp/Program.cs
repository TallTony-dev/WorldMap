using nanoFramework.WebServer;
using System;
using System.Device.Wifi;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace DeviceApp
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            WebServer server = new WebServer(80, HttpProtocol.Http);
            server.CommandReceived += HttpHandling.OnCommandRecieved;
            server.Start();

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
