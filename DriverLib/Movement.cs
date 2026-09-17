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


        public async Task MoveInDirectionUntilStopped(Direction direction)
        {
            await _transport.TransferToDevice(new DeviceCommand("MoveInDirectionUntilStopped", [direction.ToString()]));
        }

        public async Task StopMovement()
        {
            await _transport.TransferToDevice(new DeviceCommand("StopMovement", []));
        }

        public async Task MoveInDirectionForDuration(Direction direction, float duration)
        {
            await _transport.TransferToDevice(new DeviceCommand("MoveInDirectionForDuration", [duration.ToString()]));
        }
    }
}
