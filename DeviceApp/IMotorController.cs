using System;
using System.Collections;
using System.Text;

namespace DeviceApp
{
    internal interface IMotorController
    {
        public void DriveForwards();

        public void DriveBackwards();

        public void StopDriving();
    }
}
