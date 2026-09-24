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
            _frontLeft = new L298NDriverChannel();
            _frontRight = new L298NDriverChannel();
            _backLeft = new L298NDriverChannel();
            _backRight = new L298NDriverChannel();
        }


        public void StopMovement()
        {
            
        }

    }
}
