using RobotApp.Helpers;
using RobotApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RobotApp
{
    public static class Coordinator
    {
        private static InstructionsHelper _instructionsHelper;
        private static bool _initialised = false;

        /// <summary>
        /// Executes the commands from the given file
        /// </summary>
        /// <param name="filename">The file containing the list of commands</param>
        public static void Run(string filename)
        {
            Initialise(filename);
            RunScenarios();
        }

        /// <summary>
        /// Parse the commands contained in the given input file
        /// </summary>
        /// <param name="filename">The file containing the list of commands</param>
        private static void Initialise(string filename)
        {
            if (!_initialised)
            {
                _instructionsHelper = CommandParser.ParseInstructions(GetFileCommands(filename));
                _initialised = true;
            }
        }

        /// <summary>
        /// Fetch the list of commands in a file into a List<String> object
        /// Ignores any blank lines in the input file
        /// </summary>
        /// <param name="filename">The file containing the list of commands</param>
        /// <returns>A list of strings representing the commands</returns>
        /// <exception cref="FileNotFoundException">Occurs if the input file cannot be found</exception>
        private static List<string> GetFileCommands(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(ErrorMessages.FileDoesNotExist, filename);
            
            List<string> fileCommands = new List<string>();

            fileCommands = File.ReadAllLines(filename).ToList();
            fileCommands.RemoveAll(s => String.IsNullOrEmpty(s));

            return fileCommands;
        }

        /// <summary>
        /// Execute all scenarios
        /// </summary>
        /// <exception cref="NullReferenceException">Occurs if no scenarios have been loaded</exception>
        private static void RunScenarios()
        {
            if (_instructionsHelper.Scenarios == null || _instructionsHelper.Scenarios.Count == 0)
                throw new NullReferenceException(ErrorMessages.AttemptedNullScenarioRun);

            foreach (Scenario eachScenario in _instructionsHelper.Scenarios)
            {
                string result = _instructionsHelper.Grid.RunScenario(eachScenario);
                Console.WriteLine(result);
            }
        }

    }
}
