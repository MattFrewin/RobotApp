using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobotApp.Models;
using RobotApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Models.Tests
{
    [TestClass()]
    public class GridTests
    {
        [TestMethod()]
        public void RunScenarioTest()
        {
            SampleScenarioTests();
            Sample1ScenarioTests();
            Sample2ScenarioTests();
        }

        private void SampleScenarioTests()
        {
            string result;

            // Sample.txt
            Grid grid = new Grid("GRID 4x3");

            Scenario scenario = new Scenario(new string[3] { "1 1 E", "RFR", "1 0 W" });
            result = grid.RunScenario(scenario);
            Assert.IsTrue(result.Equals("SUCCESS 1 0 W"));

            scenario = new Scenario(new string[3] { "1 1 E", "RFRF", "1 1 E" });
            result = grid.RunScenario(scenario);
            Assert.IsTrue(result.Equals("FAILURE 0 0 W"));

            scenario = new Scenario(new string[3] { "1 1 E", "RFF", "1 1 E" });

            try
            {
                result = grid.RunScenario(scenario);
                Assert.Fail();  // Expecting an exception in the previous statement...
            }
            catch (IndexOutOfRangeException ex)
            {
                Assert.IsTrue(ex.Message.Equals(ErrorMessages.RobotOutOfBounds));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private void Sample1ScenarioTests()
        {
            string result;

            // Sample1.txt
            Grid grid = new Grid("GRID 40x40");

            Scenario scenario = new Scenario(new string[3] { "1 1 E", "RFRFRFRF", "1 1 E" });
            result = grid.RunScenario(scenario);
            Assert.IsTrue(result.Equals("SUCCESS 1 1 E"));

            scenario = new Scenario(new string[3] { "3 2 N", "FRRFLLFFRRFLL", "3 3 N" });
            result = grid.RunScenario(scenario);
            Assert.IsTrue(result.Equals("SUCCESS 3 3 N"));

            scenario = new Scenario(new string[3] { "0 3 W", "LLFFFLFLFL", "2 4 S" });
            result = grid.RunScenario(scenario);
            Assert.IsTrue(result.Equals("SUCCESS 2 4 S"));
        }

        private void Sample2ScenarioTests()
        {
            string result;

            // Sample2.txt
            List<string> obstacles = new List<string>() { "OBSTACLE 1 2", "OBSTACLE 1 3", "OBSTACLE 2 4" };
            Grid grid = new Grid("GRID 20x20", obstacles);

            Scenario scenario = new Scenario(new string[3] { "1 1 E", "RFRFRFRF", "1 1 E" });
            result = grid.RunScenario(scenario);
            Assert.IsTrue(result.Equals("SUCCESS 1 1 E"));

            scenario = new Scenario(new string[3] { "3 2 N", "FRRFLLFFRRFLL", "3 3 N" });
            result = grid.RunScenario(scenario);
            Assert.IsTrue(result.Equals("SUCCESS 3 3 N"));

            scenario = new Scenario(new string[3] { "0 3 W", "LLFFFLFLFL", "2 4 S" });
            result = grid.RunScenario(scenario);
            Assert.IsTrue(result.Equals("CRASHED 1 3 E"));
        }
    }
}