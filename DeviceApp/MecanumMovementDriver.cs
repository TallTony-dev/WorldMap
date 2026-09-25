using System;
using System.Collections;
using System.Text;
using System.Device.Gpio;
using System.Threading;

namespace DeviceApp
{
    internal class MecanumMovementDriver : IMovementDriver
    {
        private object _movementLock = new object();

        private const float oneEightyTimeMs = 500f;

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
            lock (_movementLock)
            {
                _frontRight.StopDriving();
                _frontRight.StopDriving();
                _backLeft.StopDriving();
                _backRight.StopDriving();
            }
        }

        public void MoveInDirectionForDuration(Direction dir, int durationMs)
        {
            lock ( _movementLock)
            {
                MoveInDirection(dir);
                Thread.Sleep(durationMs);
                StopMovement();
            }
        }

        public void MoveInDirection(Direction direction)
        {
            lock (_movementLock)
            {
                Rotate(direction);
                _frontLeft.DriveForwards();
                _backLeft.DriveForwards();
                _frontRight.DriveForwards();
                _backRight.DriveForwards();
            }
        }

        public void Rotate(Direction deltaDir)
        {
            lock (_movementLock)
            {
                if (deltaDir.GetDegrees() == 0)
                {
                    return;
                }

                bool cw = deltaDir.GetDegrees() <= 180;
                StartRotating(cw);

                float duration = Math.Abs(deltaDir.GetDegrees() - 180) * oneEightyTimeMs / 180;
                Thread.Sleep((int)duration);
                StopMovement();
            }
        }

        private void StartRotating(bool clockwise)
        {
            lock (_movementLock)
            {
                if (clockwise)
                {
                    _frontLeft.DriveForwards();
                    _backLeft.DriveForwards();
                    _frontRight.DriveBackwards();
                    _backRight.DriveBackwards();
                }
                else
                {
                    _frontLeft.DriveBackwards();
                    _backLeft.DriveBackwards();
                    _frontRight.DriveForwards();
                    _backRight.DriveForwards();
                }
            }
        }
    }
}
