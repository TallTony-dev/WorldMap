using System;
using System.Collections.Generic;
using System.Text;

namespace DriverLib
{
    public interface IMovement
    {
        /// <summary>
        /// Moves in <paramref name="direction"/> (north being forward) for <paramref name="duration"/> seconds
        /// </summary>
        /// <param name="direction">Direction to travel in, not compass aware, just north is forwards, south is backwards</param>
        /// <param name="duration">Duration to move in secs</param>
        public Task MoveInDirectionForDuration(Direction direction, float duration);

        /// <summary>
        /// Moves in <paramref name="direction"/> (north being forward) forever until <see cref="StopMovement"/> is called
        /// </summary>
        public Task MoveInDirectionUntilStopped(Direction direction);

        /// <summary>
        /// Moves in <paramref name="direction"/> (north being forward) until an object is sensed <= <paramref name="distanceFromObject"/> meters away
        /// </summary>
        public Task MoveInDirectionUntilSenseForwards(Direction direction, float distanceFromObject);

        /// <summary>
        /// Stops all active movement 
        /// </summary>
        public Task StopMovement();
    }
}
