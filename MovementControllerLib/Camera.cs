using DriverLib.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverLib
{
    internal class Camera : ICamera
    {
        private IDeviceTransport _transport;

        public Camera(IDeviceTransport transport)
        {
            _transport = transport;
        }
    }
}
