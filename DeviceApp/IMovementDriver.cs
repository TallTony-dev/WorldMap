using System;
using System.Collections;
using System.Text;

namespace DeviceApp
{
    internal interface IMovementDriver
    {
        /// <summary>
        /// Stops all current movement
        /// </summary>
        public void StopMovement();

        /// <summary>
        /// Moves in a direction relative to forwards being north until StopMovement is called
        /// </summary>
        public void MoveInDirection(Direction dir);

        /// <summary>
        /// Moves in a direction relative to forwards being north for a duration
        /// </summary>
        public void MoveInDirectionForDuration(Direction dir, int durationMs);

        public void Rotate(Direction deltaDir);

    }
}
