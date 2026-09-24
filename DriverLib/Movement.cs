using DriverLib.Transport;
using Google.Protobuf.WellKnownTypes;
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
            await _transport.TransferToDevice(new DeviceCommand(DeviceCommandType.MoveInDirectionUntilStopped, [direction.ToString()]));
        }

        public async Task StopMovement()
        {
            await _transport.TransferToDevice(new DeviceCommand(DeviceCommandType.StopMovement, []));
        }

        public async Task MoveInDirectionForDuration(Direction direction, float duration)
        {
            await _transport.TransferToDevice(new DeviceCommand(DeviceCommandType.MoveInDirectionForDuration, [duration.ToString()]));
        }

        public async Task MoveInDirectionUntilSenseForwards(Direction direction, float distanceFromObject)
        {
            await _transport.TransferToDevice(new DeviceCommand(DeviceCommandType.MoveInDirectionUntilSense, [distanceFromObject.ToString()]));
        }

    }
}
