using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RobotApp.Models.Tests
{
    [TestClass()]
    public class ScenarioTests
    {
        [TestMethod()]
        public void ProcessNextInstructionTest()
        {
            string[] scenarioCommands = new string[3] {
                "0 0 N",
                "FRFRFRFRLLLL",
                "0 0 N"  // End position is irrelevant for this test
            };

            Scenario scenario = new Scenario(scenarioCommands);

            scenario.ProcessNextInstruction(); // F
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 0, Y = 1, Direction = Sprite.Heading.North }));

            scenario.ProcessNextInstruction(); // R
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 0, Y = 1, Direction = Sprite.Heading.East }));

            scenario.ProcessNextInstruction(); // F
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 1, Y = 1, Direction = Sprite.Heading.East }));

            scenario.ProcessNextInstruction(); // R
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 1, Y = 1, Direction = Sprite.Heading.South }));

            scenario.ProcessNextInstruction(); // F
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 1, Y = 0, Direction = Sprite.Heading.South }));

            scenario.ProcessNextInstruction(); // R
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 1, Y = 0, Direction = Sprite.Heading.West }));

            scenario.ProcessNextInstruction(); // F
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 0, Y = 0, Direction = Sprite.Heading.West }));

            scenario.ProcessNextInstruction(); // R
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 0, Y = 0, Direction = Sprite.Heading.North }));

            scenario.ProcessNextInstruction(); // L
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 0, Y = 0, Direction = Sprite.Heading.West }));

            scenario.ProcessNextInstruction(); // L
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 0, Y = 0, Direction = Sprite.Heading.South }));

            scenario.ProcessNextInstruction(); // L
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 0, Y = 0, Direction = Sprite.Heading.East }));

            scenario.ProcessNextInstruction(); // L
            Assert.IsTrue(scenario.CurrentPosition.Equals(new Sprite() { X = 0, Y = 0, Direction = Sprite.Heading.North }));
        }


    }
}