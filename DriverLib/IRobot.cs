using System;
using System.Collections.Generic;
using System.Text;

namespace DriverLib
{
    public interface IRobot //lol like the movie
    {

        public Task<Image> GetImageFromDirection(Direction direction);

        public Task MoveInDirection(Direction movementDir, float durationSecs);

        /// <summary>
        /// Move the robot in a direction until an object is detected in a direction a certain distance away
        /// </summary>
        /// <param name="movementDir">Direction to move</param>
        /// <param name="distanceFromObject">The distance from the object for which to stop moving</param>
        public Task MoveInDirectionUntilSenseForwards(Direction movementDir, int distanceFromObject);

        /// <summary>
        /// Gets the direction that corresponds to the front of the robot via compass
        /// </summary>
        public Direction GetDirectionOfFront();

    }
}
