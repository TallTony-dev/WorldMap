using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Device.Wifi;
using nanoFramework.WebServer;

namespace DeviceApp
{
    internal class HttpHandling
    {
        

        public static void OnCommandRecieved(object source, WebServerEventArgs args)
        {
            var url = args.Context.Request.RawUrl.ToLower();
            
            if (url == "/command")
            {

            }

        }




    }

    internal struct DeviceCommand
    {
        public string CommandName { get; set; }
        public string[] Args { get; set; }

        public DeviceCommand(string commandName, string[] args)
        {
            CommandName = commandName;
            Args = args;
        }
    }
}
