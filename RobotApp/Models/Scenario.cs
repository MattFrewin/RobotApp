using RobotApp.Helpers;
using System.IO;

namespace RobotApp.Models
{
    public class Scenario
    {
        #region Public Attributes
        
        /// <summary>
        /// The current position of the robot
        /// </summary>
        public Sprite CurrentPosition;

        /// <summary>
        /// The expected end position of the robot
        /// </summary>
        public Sprite ExpectedEndPosition;
        
        #endregion

        #region Private Attributes

        /// <summary>
        /// An ordered list of instructions for the robot to attempt to process
        /// </summary>
        private char[] _journey;

        /// <summary>
        /// Pointer to the next instruction for the robot to execute
        /// </summary>
        private int _nextInstruction = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new scenario object using the given scenario declaration commands
        /// </summary>
        /// <param name="scenarioCommands">An array of commands describing a scenario</param>
        /// <exception cref="InvalidDataException">Occurs when given an invalid scenario declaration command</exception>
        public Scenario(string[] scenarioCommands)
        {
            if(scenarioCommands.Length != CommandParser.ScenarioCommandLength)
                throw new InvalidDataException(ErrorMessages.InvalidScenarioDeclaration);

            // The first line of the scenario must be the start position, use this to initialise the current position
            // The second line of the scenario must be the robot's journey
            // The third line of the scenario must be the expected end position
            CurrentPosition = CommandParser.ParsePosition(scenarioCommands[0]);
            _journey = CommandParser.ParseJourney(scenarioCommands[1]);
            ExpectedEndPosition = CommandParser.ParsePosition(scenarioCommands[2]);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Process the next instruction in the journey
        /// </summary>
        /// <returns>True, if additional instructions remain in the journey after the current instruction is processed. <see langword="false"/>otherwise</returns>
        /// <exception cref="InvalidDataException">Occurs when the journey is <see langword="null"/>or empty</exception>
        public bool ProcessNextInstruction()
        {
            if (_journey == null || _journey.Length == 0)
                throw new InvalidDataException(ErrorMessages.InvalidScenarioDeclaration);

            if (_nextInstruction >= _journey.Length)
                return false;

            char nextInstruction = _journey[_nextInstruction++];
            switch (nextInstruction)
            {
                case Instruction.Left:
                    CurrentPosition.Turn(Sprite.TurnDirection.Left);
                    break;
                case Instruction.Right:
                    CurrentPosition.Turn(Sprite.TurnDirection.Right);
                    break;
                case Instruction.Forward:
                    CurrentPosition.MoveForward();
                    break;
            }

            return true;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Helper class for scenario instructions
        /// </summary>
        private class Instruction
        {
            public const char Left = 'L';
            public const char Right = 'R';
            public const char Forward = 'F';
        };

        #endregion
    }
}
