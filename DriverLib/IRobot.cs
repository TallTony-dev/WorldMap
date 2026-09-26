using System;
using System.Collections.Generic;
using System.Text;

namespace DriverLib
{
    public interface IRobot //lol like the movie
    {

        public Task<Image> GetImageFromDirection(Direction direction);

        public Task MoveInDirection(Direction movementDir, int durationMs);

        /// <summary>
        /// Move the robot in a direction until an object is detected in a direction a certain distance away
        /// </summary>
        /// <param name="movementDir">Direction to move</param>
        /// <param name="distanceFromObject">The distance from the object for which to stop moving in meters</param>
        public Task MoveInDirectionUntilSenseForwards(Direction movementDir, float distanceFromObject);

        /// <summary>
        /// Gets the direction that corresponds to the front of the robot via compass
        /// </summary>
        public Direction GetDirectionOfFront();

        /// <summary>
        /// Calls an agent to ensure surroundings are properly graphed, should be repeatedly called every so often
        /// </summary>
        public Task GraphSurroundings();

    }
}
