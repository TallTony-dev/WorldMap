using System;
using System.Collections;
using System.Text;
using System.Device.Gpio;

namespace DeviceApp
{
    internal class MecanumMovementDriver : IMovementDriver
    {
        private object _movementLock = new object();

        //currently using L298N driver

        private IMotorController _frontLeft;
        private IMotorController _frontRight;
        private IMotorController _backLeft;
        private IMotorController _backRight;


        public MecanumMovementDriver()
        {
            _frontLeft = new L298NDriverChannel(13, 12, 14);
            _frontRight = new L298NDriverChannel(27, 26, 25);
            _backLeft = new L298NDriverChannel(33, 32, 35);
            _backRight = new L298NDriverChannel(34, 39, 36);
        }


        public void StopMovement()
        {
            
        }

        public void MoveInDirection(Direction direction)
        {

        }

    }
}
