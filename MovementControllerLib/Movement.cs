using DriverLib.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriverLib
{
    internal class Movement : IMovement
    {
        private IDeviceTransport _transport;

        public Movement(IDeviceTransport transport)
        {
            _transport = transport;
        }

    }
}
