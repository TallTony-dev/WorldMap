using System;
using System.Collections;
using System.Text;
using System.Device.Gpio;

namespace DeviceApp
{
    internal class MecanumMovementDriver : IMovementDriver
    {
        private object _movementLock = new object();
        private readonly GpioController _gpioController = new GpioController();

        //currently using L298N driver

        private IMotorController _frontLeft;
        private IMotorController _frontRight;
        private IMotorController _backLeft;
        private IMotorController _backRight;


        public MecanumMovementDriver()
        {
            _frontLeft = new L298NDriver();
            _frontRight = new L298NDriver();
            _backLeft = new L298NDriver();
            _backRight = new L298NDriver();
        }


        public void StopMovement()
        {
            
        }

    }
}
