using RobotApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RobotApp.Helpers 
{
    public class CommandParser
    {
        #region Private Attributes

        /// <summary>
        /// The expected prefix for the file command to instatiate a new grid
        /// </summary>
        private const string _gridDeclarationCommand = "GRID ";

        /// <summary>
        /// The expected delimiter between the width and height values in a GRID command
        /// </summary>
        private const string _gridSizeDelimiter = "x";

        /// <summary>
        /// The expected prefix for the file command to instatiate a new obstacle
        /// </summary>
        private const string _obstacleDeclarationCommand = "OBSTACLE ";

        /// <summary>
        /// The expected delimiter between the x and y values in an OBSTACLE command
        /// </summary>
        private const string _obstacleLocationDelimiter = " ";

        #endregion

        #region Public Attributes

        /// <summary>
        /// A scenario consists of 3 lines in the command file (the robot's start location, the path to traverse, and the robot's end location)
        /// </summary>
        public const int ScenarioCommandLength = 3;

        #endregion

        #region Helpers

        /// <summary>
        /// Helper class containing regular expressions for validating input commands
        /// </summary>
        public class CommandRegularExpressions
        {
            /// <summary>
            /// Regular expression for "GRID " followed by 1 or more digits, then an 'x', then 1 or more digits (e.g. "GRID 5x2" or "GRID 24x150" etc...) 
            /// </summary>
            public const string ExpectedGridDeclarationFormat = "^" + _gridDeclarationCommand + @"\d+" + _gridSizeDelimiter + @"\d+$";

            /// <summary>
            /// Regular expression for a sequence of characters from the set { L, R, F }
            /// </summary>
            public const string ExpectedRobotJourneyDeclarationFormat = "^[LRF]+$";

            /// <summary>
            /// Regular expression for:
            ///     - a number, followed by
            ///     - a space, followed by
            ///     - another number, followed by
            ///     - another space, followed by
            ///     - one of the following characters N, E, S, W
            /// E.g. "3 5 N", "14, 4 W", etc...
            /// </summary>
            public const string ExpectedRobotPositionDeclarationFormat = @"^\d+ \d+ [NESW]$";

            /// <summary>
            /// Regular expression for a sequence of characters from the set { L, R, F }
            /// </summary>
            public const string ExpectedObstacleDeclarationFormat = "^" + _obstacleDeclarationCommand + @"\d+" + _obstacleLocationDelimiter + @"\d+$";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parse an instructions command list into a Grid object with a list of Obstacle objects, and a list of Scenario objects
        /// </summary>
        /// <param name="instructions">The input instruction command list</param>
        /// <returns>The Grid with any obstacles, and a list of scenarios taken from the instructions command list</returns>
        /// <exception cref="InvalidDataException">Occurs if the input list is null or empty</exception>
        public static InstructionsHelper ParseInstructions(List<string> instructions)
        {
            // Check the file commands have been loaded
            if (instructions == null || instructions.Count <= 0)
                throw new InvalidDataException(ErrorMessages.FileCommandsNotLoaded);

            InstructionsHelper instructionsHelper = new InstructionsHelper();

            // First command must be a GRID declaration
            string gridCommand = instructions[0];
            instructions.RemoveAt(0);

            // Check for any OBSTACLE commands
            List<string> obstacleCommands = GetObstacleCommands(instructions);

            // Create the grid with any obstacles
            instructionsHelper.Grid = new Grid(gridCommand, obstacleCommands);

            // The remaining number of commands should be an exact multiple of the number of commands required to describe a scenario (in this case, 3 commands per scenario)
            if ((instructions.Count % ScenarioCommandLength) != 0)
                throw new InvalidDataException(ErrorMessages.InvalidScenarioDeclaration);

            // Create the scenarios and return them with the grid
            instructionsHelper.Scenarios = GetScenarioCommands(instructions);

            return instructionsHelper;
        }

        /// <summary>
        /// Parse a command containing position and direction into a Sprite object
        /// </summary>
        /// <param name="positionCommand">The input command containing position and direction</param>
        /// <returns>A new Sprite object with position and direction taken from the input command string</returns>
        /// <exception cref="InvalidDataException">Occurs if the input command is not in the expected format</exception>
        public static Sprite ParsePosition(string positionCommand)
        {
            // Validate the command
            if (!Regex.IsMatch(positionCommand, CommandRegularExpressions.ExpectedRobotPositionDeclarationFormat))
                throw new InvalidDataException(ErrorMessages.InvalidRobotPositionDeclaration_WithInputCommand + positionCommand);

            // The expected format uses a space character as a delimiter for the X,Y and Direction values
            string[] positionDetails = positionCommand.Split(" ");

            // Create and return a sprite with the values found above
            return new Sprite() {
                X = Convert.ToInt32(positionDetails[0]),
                Y = Convert.ToInt32(positionDetails[1]),
                Direction = Convert.ToChar(positionDetails[2])
            };
        }

        /// <summary>
        /// Extract the dimensions from a GRID command and return them as a Point object
        /// </summary>
        /// <param name="gridCommand">The input grid command string</param>
        /// <returns>A new Point object with its X,Y set to the grid dimensions from the input command string</returns>
        /// <exception cref="InvalidDataException">Occurs if the grid command is not in the expected format</exception>
        public static Point ParseGridDimensions(string gridCommand)
        {
            // Validate the command
            if (!Regex.IsMatch(gridCommand, CommandRegularExpressions.ExpectedGridDeclarationFormat))
                throw new InvalidDataException(ErrorMessages.InvalidGridDeclaration);

            // Extract and return the grid dimensions
            return ExtractPoint(gridCommand, _gridDeclarationCommand, _gridSizeDelimiter);
        }

        /// <summary>
        /// Parse a journey command string into a list of single instructions
        /// </summary>
        /// <param name="journeyCommand">The input journey command string</param>
        /// <returns>An ordered list of chars each representing an instruction in the journey</returns>
        /// <exception cref="InvalidDataException">Occurs if the journey command is not in the expected format</exception>
        public static char[] ParseJourney(string journeyCommand)
        {
            // Validate the command
            if (!Regex.IsMatch(journeyCommand, CommandRegularExpressions.ExpectedRobotJourneyDeclarationFormat))
                throw new InvalidDataException(ErrorMessages.InvalidScenarioDeclaration);

            // Split the string into a list of chars
            return journeyCommand.ToArray();
        }

        /// <summary>
        /// Create a Point object to represent an obstacle
        /// </summary>
        /// <param name="obstacleCommand">The input obstacle command string</param>
        /// <returns>A new Point object with its X,Y taken from the input command string</returns>
        /// <exception cref="InvalidDataException">Occurs if the obstacle command is not in the expected format</exception>
        public static Point ParseObstacle(string obstacleCommand)
        {
            // Validate the command
            if (!Regex.IsMatch(obstacleCommand, CommandRegularExpressions.ExpectedObstacleDeclarationFormat))
                throw new InvalidDataException(ErrorMessages.InvalidObstacleDeclaration);

            // Extract and return the obstacle location
            return ExtractPoint(obstacleCommand, _obstacleDeclarationCommand, _obstacleLocationDelimiter);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Extract a list of obstacle commands from the given instruction command list
        /// Also removes the obstacle commands from the instructions list
        /// </summary>
        /// <param name="instructions">The input instruction command list</param>
        /// <returns>The list of obstacle commands</returns>
        private static List<string> GetObstacleCommands(List<string> instructions)
        {
            List<string> obstacleCommands = new List<string>();

            // Continue while the command is an OBSTACLE
            while (instructions[0].StartsWith("OBSTACLE"))
            {
                obstacleCommands.Add(instructions[0]);  // Extract the obstacle...
                instructions.RemoveAt(0);               // ... and remove it from the list
            }

            return obstacleCommands;
        }

        /// <summary>
        /// Parse a list of instruction commands into a list of Scenario objects.
        /// </summary>
        /// <param name="instructions">The input commands, could contain 1 or many scenario definitions</param>
        /// <returns>A list of Scenario object based on the input commands</returns>
        private static List<Scenario> GetScenarioCommands(List<string> instructions)
        {
            // Initialise the scenario list
            List<Scenario> scenarios = new List<Scenario>();

            // Parse each scenario
            while (instructions.Count > 0)
            {
                // Take the next 3 commands (which describe a single scenario)
                string[] scenarioCommands = instructions.Take(ScenarioCommandLength).ToArray();

                // Create the Scenario object 
                scenarios.Add(new Scenario(scenarioCommands));

                // Remove the current scenario commands from the list of commands, ready to parse the next scenario
                instructions.RemoveRange(0, ScenarioCommandLength);
            }

            return scenarios;
        }

        /// <summary>
        /// Create a Point object from a string containing a command and an X,Y pair separated by a given delimiter
        /// </summary>
        /// <param name="command">The input command string</param>
        /// <param name="prefix">The command prefix before the X,Y pair</param>
        /// <param name="delimiter">The delimiter between the X and Y values</param>
        /// <returns>A new Point object with its X,Y taken from the input command string</returns>
        private static Point ExtractPoint(string command, string prefix, string delimiter)
        {
            // Remove the declaration command prefix and then get the Point
            command = command.Replace(prefix, "");
            int[] point = command.Split(delimiter).Select(s => Convert.ToInt32(s)).ToArray();

            // Create and return a Point object with the values found above
            return new Point(point[0], point[1]);
        }

        #endregion region
    }
}
