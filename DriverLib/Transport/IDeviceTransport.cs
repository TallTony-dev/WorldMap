using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace DriverLib.Transport
{
    internal interface IDeviceTransport
    {

        public Task TransferToDevice(DeviceCommand command);

        public Task<RecievedDeviceData> GetFromDevice(DeviceCommand getCommand);

    }
}
