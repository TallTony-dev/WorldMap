using System;
using System.Collections;
using System.Text;

namespace DeviceApp
{
    internal interface IMovementDriver
    {

        public void StopMovement();
        /// <summary>
        /// Moves in a direction relative to forwardsd being north
        /// </summary>
        /// <param name="dir"></param>
        public void MoveInDirection(Direction dir);
        

    }
}
