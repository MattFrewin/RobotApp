using RobotApp.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace RobotApp.Models
{
    /// <summary>
    /// Representation of a grid
    /// </summary>
    public class Grid
    {
        #region Public Attributes

        /// <summary>
        /// Grid dimensions
        /// </summary>
        public Point Dimensions { get; set; }

        /// <summary>
        /// List of obstacles in the grid
        /// </summary>
        private List<Point> _obstacles;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new grid object using the given grid declaration command
        /// </summary>
        /// <param name="gridDeclaration">A grid declaration command</param>
        /// <exception cref="InvalidDataException">Occurs when given an invalid grid declaration command</exception>
        public Grid(string gridDeclaration)
        {
            Dimensions = CommandParser.ParseGridDimensions(gridDeclaration);
            _obstacles = new List<Point>();
        }

        /// <summary>
        /// Initialise a new grid object using the given grid declaration command and obstacle command list
        /// </summary>
        /// <param name="gridDeclaration">A grid declaration command</param>
        /// <param name="obstacleDeclarations">A list of obstacle declaration commands</param>
        /// <exception cref="InvalidDataException">Occurs when given an invalid grid declaration command</exception>
        public Grid(string gridDeclaration, List<string> obstacleDeclarations) : this(gridDeclaration)
        {
            _obstacles = obstacleDeclarations.Select(o => CommandParser.ParseObstacle(o)).ToList();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Processes instructions in the journey for a given Scenario.
        /// If the robot leaves the grid or hits and obstacle then the scenario is ended with an appropriate message
        /// </summary>
        /// <param name="scenario">The Scenario to run</param>
        /// <returns>A message describing the result of running this Scenario</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public string RunScenario(Scenario scenario)
        {
            do
            {
                // Check the robot hasn't left the grid
                if (!IsRobotInValidPosition(scenario.CurrentPosition))
                    throw new IndexOutOfRangeException(ErrorMessages.RobotOutOfBounds);

                // Check the robot hasn't crashed into an obstacle
                if(HasRobotHitObstacle(scenario.CurrentPosition))
                    return (ErrorMessages.RobotObstacleCrash + " " + scenario.CurrentPosition.ToString());

            } while (scenario.ProcessNextInstruction());

            // Return the success or failure for this scenario
            string result = scenario.CurrentPosition.Equals(scenario.ExpectedEndPosition) ? "SUCCESS " : "FAILURE ";
            result += scenario.CurrentPosition.ToString();

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks the robot is inside the grid
        /// </summary>
        /// <param name="robotPosition">The current position of the robot</param>
        /// <returns>True, if the robot is in the grid. False otherwise</returns>
        private bool IsRobotInValidPosition(Sprite robotPosition)
        {
            return
                robotPosition.X >= 0 && robotPosition.X < Dimensions.X &&
                robotPosition.Y >= 0 && robotPosition.Y < Dimensions.Y;
        }

        /// <summary>
        /// Checks if the robot has crashed into an obstacle in the grid
        /// </summary>
        /// <param name="robotPosition">The current position of the robot</param>
        /// <returns>True, if the robot has crashed into an obstacle. False otherwise</returns>
        private bool HasRobotHitObstacle(Sprite robotPosition)
        {
            return _obstacles.Exists(o => robotPosition.Equals(o));
        }

        #endregion
    }
}
