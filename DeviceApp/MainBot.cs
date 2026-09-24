using System;
using System.Collections;
using System.Device.Gpio;
using System.Text;

namespace DeviceApp
{
    internal static class MainBot
    {
        public static IMovementDriver MovementDriver { get; private set; } = new MecanumMovementDriver();
        public static GpioController GpioController { get; private set; }  = new GpioController();


    }
}
