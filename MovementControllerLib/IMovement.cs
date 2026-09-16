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
        /// Moves in <paramref name="direction"/> (north being forward) until <paramref name="condition"/> is set to true
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="condition"></param>
        public Task MoveInDirectionUntilCondition(Direction direction, ref bool condition);
        
    }
}
