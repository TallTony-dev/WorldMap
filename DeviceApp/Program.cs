using nanoFramework.Networking;
using nanoFramework.WebServer;
using System;
using System.Device.Gpio;
using System.Device.Wifi;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace DeviceApp
{
    public class Program
    {

        const string ssid = "BELL924";
        const string password = "EDCFE1159CD4";

        private static GpioPin led;

        private static void Blink(int count)
        {
            GpioPin led = MainBot.GpioController.OpenPin(2, PinMode.Output);
            for (int i = 0; i < count; i++ )
            {
                led.Toggle();
                Thread.Sleep(125);
                led.Toggle();
                Thread.Sleep(125);
            }
            
        }

        public static void Main()
        {

            
            Debug.WriteLine("Hello from nanoFramework!");

            var gpioController = new GpioController();

            led = gpioController.OpenPin(2, PinMode.Output);

            led.Write(PinValue.Low);
            led.Toggle();
            Thread.Sleep(125);
            led.Toggle();
            Thread.Sleep(125);
            led.Toggle();
            Thread.Sleep(525);
            led.Toggle();

            WifiNetworkHelper.ConnectDhcp(ssid, password);
            led.Toggle();

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            if (interfaces.Length > 0)
            {
                NetworkInterface ni = interfaces[0];

                Debug.WriteLine($"IP Address: {ni.IPv4Address}");
                Debug.WriteLine($"Subnet Mask: {ni.IPv4SubnetMask}");
                Debug.WriteLine($"Gateway: {ni.IPv4GatewayAddress}");
            }

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
