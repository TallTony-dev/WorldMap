using System;
using System.Collections;
using System.Text;

namespace DeviceApp
{
    internal interface IMotorController
    {
        public void DriveForwards(double dutyCycle = 1);

        public void DriveBackwards(double dutyCycle = 1);

        public void StopDriving();
    }
}
