using System;
using System.Collections;
using System.Text;

namespace DeviceApp
{
    internal static class MainBot
    {
        public static IMovementDriver MovementDriver { get; private set; } = new MecanumMovementDriver();



    }
}
