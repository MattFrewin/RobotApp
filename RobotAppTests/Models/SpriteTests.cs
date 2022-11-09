using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Models.Tests
{
    [TestClass()]
    public class SpriteTests
    {
        [TestMethod()]
        public void TurnTest()
        {
            Sprite sprite = new Sprite() { Direction = Sprite.Heading.North };

            // Turning 'Right':            
            sprite.Turn(Sprite.TurnDirection.Right);
            Assert.AreEqual(sprite.Direction, Sprite.Heading.East);

            sprite.Turn(Sprite.TurnDirection.Right);
            Assert.AreEqual(sprite.Direction, Sprite.Heading.South);

            sprite.Turn(Sprite.TurnDirection.Right);
            Assert.AreEqual(sprite.Direction, Sprite.Heading.West);

            sprite.Turn(Sprite.TurnDirection.Right);
            Assert.AreEqual(sprite.Direction, Sprite.Heading.North);


            // Turning 'Left':            
            sprite.Turn(Sprite.TurnDirection.Left);
            Assert.AreEqual(sprite.Direction, Sprite.Heading.West);

            sprite.Turn(Sprite.TurnDirection.Left);
            Assert.AreEqual(sprite.Direction, Sprite.Heading.South);

            sprite.Turn(Sprite.TurnDirection.Left);
            Assert.AreEqual(sprite.Direction, Sprite.Heading.East);

            sprite.Turn(Sprite.TurnDirection.Left);
            Assert.AreEqual(sprite.Direction, Sprite.Heading.North);

        }

        [TestMethod()]
        public void MoveForwardTest()
        {
            Sprite sprite = new Sprite() { X = 0, Y = 0 };

            // Move North
            sprite.Direction = Sprite.Heading.North;
            sprite.MoveForward();
            Assert.IsTrue(sprite.Equals(new Sprite() { X = 0, Y = 1, Direction = Sprite.Heading.North }));

            // Move East
            sprite = new Sprite() { X = 0, Y = 0 };
            sprite.Direction = Sprite.Heading.East;
            sprite.MoveForward();
            Assert.IsTrue(sprite.Equals(new Sprite() { X = 1, Y = 0, Direction = Sprite.Heading.East }));

            // Move South
            sprite = new Sprite() { X = 0, Y = 0 };
            sprite.Direction = Sprite.Heading.South;
            sprite.MoveForward();
            Assert.IsTrue(sprite.Equals(new Sprite() { X = 0, Y = -1, Direction = Sprite.Heading.South }));

            // Move West
            sprite = new Sprite() { X = 0, Y = 0 };
            sprite.Direction = Sprite.Heading.West;
            sprite.MoveForward();
            Assert.IsTrue(sprite.Equals(new Sprite() { X = -1, Y = 0, Direction = Sprite.Heading.West }));
        }
    }
}