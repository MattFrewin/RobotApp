using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Helpers
{
    /// <summary>
    /// Helper class for error messages
    /// </summary>
    public class ErrorMessages
    {
        public static string FileDoesNotExist = "Unable to locate command file.";
        public static string FileCommandsNotLoaded = "Unable to parse file commands as they have not been loaded.";

        public static string InvalidGridDeclaration = "Command file must start with a valid GRID command, please check the file.\nAn example of the correct format is \"GRID 4x3\".";
        public static string InvalidObstacleDeclaration = $"Invalid obstacle declaration command, please check the input file.";
        public static string InvalidScenarioDeclaration = $"A scenario must consist of {CommandParser.ScenarioCommandLength} command lines, please check the input file.";
        public static string InvalidRobotPositionDeclaration = "Invalid robot position command.\nAn example of the correct format is \"5 4 W\".";
        public static string InvalidRobotPositionDeclaration_WithInputCommand = "Invalid robot position command.\nAn example of the correct format is \"5 4 W\".\nCommand given: ";

        public static string AttemptedNullScenarioRun = "Unable to run scenario, no scenarios have been loaded.";

        public static string RobotOutOfBounds = "OUT OF BOUNDS";
        public static string RobotObstacleCrash = "CRASHED";
    }
}
