using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobotApp.Models;
using System.Drawing;

namespace RobotApp.Helpers.Tests
{
    [TestClass()]
    public class CommandParserTests
    {
        [TestMethod()]
        public void ParseObstacleTest()
        {
            string obstacleCommand = "OBSTACLE 4 7";
            Point expectedObstacle = new Point(4, 7);

            Assert.IsTrue(expectedObstacle.Equals(CommandParser.ParseObstacle(obstacleCommand)));

            obstacleCommand = "NOT AN OBSTACLE 4 7";

            try
            {
                CommandParser.ParseObstacle(obstacleCommand);
                Assert.Fail();  // Expecting an exception in the previous statement...
            }
            catch (InvalidDataException ex)
            {
                Assert.IsTrue(ex.Message.Equals(ErrorMessages.InvalidObstacleDeclaration));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void ParseJourneyTest()
        {
            string journeyCommand = "FFLRLRF";
            char[] expectedJourney = new char[7] { 'F', 'F', 'L', 'R', 'L', 'R', 'F' };

            char[] parsedJourney = CommandParser.ParseJourney(journeyCommand);

            Assert.AreEqual(expectedJourney[0], parsedJourney[0]);
            Assert.AreEqual(expectedJourney[1], parsedJourney[1]);
            Assert.AreEqual(expectedJourney[2], parsedJourney[2]);
            Assert.AreEqual(expectedJourney[3], parsedJourney[3]);
            Assert.AreEqual(expectedJourney[4], parsedJourney[4]);
            Assert.AreEqual(expectedJourney[5], parsedJourney[5]);
            Assert.AreEqual(expectedJourney[6], parsedJourney[6]);

            journeyCommand = "FLR NOT A JOURNEY FRL";

            try
            {
                CommandParser.ParseJourney(journeyCommand);
                Assert.Fail();  // Expecting an exception in the previous statement...
            }
            catch (InvalidDataException ex)
            {
                Assert.IsTrue(ex.Message.Equals(ErrorMessages.InvalidScenarioDeclaration));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void ParseGridDimensionsTest()
        {
            string gridCommand = "GRID 22x46";
            Point expectedGridDimensions = new Point(22, 46);

            Assert.IsTrue(expectedGridDimensions.Equals(CommandParser.ParseGridDimensions(gridCommand)));

            gridCommand = "NOT A GRID 22x46";

            try
            {
                CommandParser.ParseGridDimensions(gridCommand);
                Assert.Fail();  // Expecting an exception in the previous statement...
            }
            catch (InvalidDataException ex)
            {
                Assert.IsTrue(ex.Message.Equals(ErrorMessages.InvalidGridDeclaration));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        [TestMethod()]
        public void ParsePositionTest()
        {
            string positionCommand = "2 6 S";
            Sprite expectedSprite = new Sprite() { X = 2, Y = 6, Direction = Sprite.Heading.South };

            Assert.IsTrue(expectedSprite.Equals(CommandParser.ParsePosition(positionCommand)));

            positionCommand = "2 6 S NOT A SPRITE 2 6 S";

            try
            {
                CommandParser.ParsePosition(positionCommand);
                Assert.Fail();  // Expecting an exception in the previous statement...
            }
            catch (InvalidDataException ex)
            {
                Assert.IsTrue(ex.Message.Equals(ErrorMessages.InvalidRobotPositionDeclaration_WithInputCommand + positionCommand));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}