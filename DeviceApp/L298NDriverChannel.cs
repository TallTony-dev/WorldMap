using System;
using System.Collections;
using System.Text;
using System.Device.Gpio;
using System.Device.Pwm;
using nanoFramework.Hardware.Esp32;

namespace DeviceApp
{
    internal class L298NDriverChannel : IMotorController
    {
        PwmChannel _pwmChannel;
        GpioPin _in1Pin;
        GpioPin _in2Pin;


        public L298NDriverChannel(int pwmPin, int in1Pin, int in2Pin)
        {
            Configuration.SetPinFunction(pwmPin, DeviceFunction.PWM1);
            _pwmChannel = PwmChannel.CreateFromPin(pwmPin);
            _in1Pin = MainBot.GpioController.OpenPin(in1Pin, PinMode.Output);
            _in2Pin = MainBot.GpioController.OpenPin(in2Pin, PinMode.Output);
        }


        public void StopDriving()
        {
            _in1Pin.Write(PinValue.Low);
            _in2Pin.Write(PinValue.Low);
            _pwmChannel.Stop();
        }

        public void DriveForwards(double dutyCycle = 1)
        {
            _pwmChannel.Start();
            _in1Pin.Write(PinValue.High);
            _in2Pin.Write(PinValue.Low);
            _pwmChannel.DutyCycle = dutyCycle;
        }

        public void DriveBackwards(double dutyCycle = 1)
        {
            _pwmChannel.Start();
            _in1Pin.Write(PinValue.Low);
            _in2Pin.Write(PinValue.High);
            _pwmChannel.DutyCycle = dutyCycle;
        }
    }
}
