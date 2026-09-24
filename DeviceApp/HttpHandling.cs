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
using nanoFramework.Json;

namespace DeviceApp
{
    internal class HttpHandling
    {
        

        public static void OnCommandRecieved(object source, WebServerEventArgs args)
        {
            var url = args.Context.Request.RawUrl.ToLower();
            
            if (url == "/command")
            {
                DeviceCommand command = (DeviceCommand)JsonConvert.DeserializeObject(args.Context.Request.InputStream, typeof(DeviceCommand));

                switch(command.CommandName)
                {
                    case (DeviceCommandType.GetImageFromDirection):

                        break;
                    case (DeviceCommandType.StopMovement):

                        break;
                    case (DeviceCommandType.MoveInDirectionUntilStopped):
                        MainBot.MovementDriver.MoveInDirection(new Direction(float.Parse(command.Args[0])));
                        break;
                    case (DeviceCommandType.MoveInDirectionForDuration):

                        break;
                    case (DeviceCommandType.MoveInDirectionUntilSense):

                        break;
                    default:
                        Debug.WriteLine("Fell out of command recieving switch");
                        break;
                }
            }

        }




    }

}
