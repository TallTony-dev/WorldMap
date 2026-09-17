using DriverLib.Transport;
using System;
using System.Collections.Generic;
using System.Text;
using static DriverLib.Direction;

namespace DriverLib
{
    public class Robot : IRobot
    {
        IMovement _movementDriver;
        ICamera _camera;
        IDeviceTransport _deviceTransport;
        
        public Robot(string targetIp)
        {
            _deviceTransport = new HttpDevice(targetIp);

            _movementDriver = new Movement(_deviceTransport);
            _camera = new Camera(_deviceTransport);
        }

        public async Task<Image> GetImageFromDirection(Direction direction)
        {
            return await _camera.GetImageFromDirection(direction);
        }

        public async Task MoveInDirection(Direction movementDir, float durationSecs)
        {
            await _movementDriver.MoveInDirectionForDuration(movementDir, durationSecs);
        }


        public async Task MoveInDirectionUntilSense(Direction movementDir, Direction objectDirection, int distanceFromObject)
        {
            
        }


        public Direction GetDirectionOfFront()
        {
            throw new NotImplementedException();
        }
    }

}
