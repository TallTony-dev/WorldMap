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

        public async Task<Image> GetImageFromFront()
        {
            return await GetImageFromDirection(new Direction(Direction.EDirection.North));
        }

        public async Task<Image> GetImageFromDirection(Direction direction)
        {
            var recieved = await _transport.GetFromDevice(new DeviceCommand(DeviceCommandType.GetImageFromDirection, new[] { $"{direction.GetDegrees()}" }));
            if (recieved?.DataType == "image")
            {
                return new Image() { ImageType = "jpeg", Data = recieved?.Data ?? Array.Empty<byte>() };
            }
            else
            {
                throw new BadImageFormatException($"Expected image from device, got {recieved?.DataType}");
            }
        }
    }
}
