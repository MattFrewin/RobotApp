using System;
using System.Drawing;

namespace RobotApp.Models
{
    public class Sprite : IEquatable<Sprite>
    {
        #region Public Attributes

        public int X { get; set; }  // X-coordinate
        public int Y { get; set; }  // Y-coordinate
        public char Direction { get; set; }   // Direction (either N, E, S or W)

        /// <summary>
        /// Returns an "arrow" character, pointing in the current direction
        /// </summary>
        public char SpriteIcon
        {
            get
            { 
                switch (Direction)
                {
                    case Heading.North: return '\u2191';
                    case Heading.East: return '\u2192';
                    case Heading.South: return '\u2193';
                    case Heading.West: return '\u2190';
                    default: return 'E';
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Turn 90 degrees (left or right) from the current direction
        /// </summary>
        /// <param name="direction">The direction to turn</param>
        public void Turn(TurnDirection direction)
        {
            if(direction == TurnDirection.Left)
            {
                // Turning left is anti-clockwise
                switch (Direction)
                {
                    case Heading.North: Direction = Heading.West; break;
                    case Heading.East: Direction = Heading.North; break;
                    case Heading.South: Direction = Heading.East; break;
                    case Heading.West: Direction = Heading.South; break;
                    default: break;
                }
            }
            else
            {
                // Turning right is clockwise
                switch (Direction)
                {
                    case Heading.North: Direction = Heading.East; break;
                    case Heading.East: Direction = Heading.South; break;
                    case Heading.South: Direction = Heading.West; break;
                    case Heading.West: Direction = Heading.North; break;
                    default: break;
                }
            } 
        }

        /// <summary>
        /// Increment the appropriate X or Y position, depending on which direction is being faced
        /// </summary>
        public void MoveForward()
        {
            switch (Direction)
            {
                case Heading.North: Y++; break;
                case Heading.East: X++; break;
                case Heading.South: Y--; break;
                case Heading.West: X--; break;
                default: break;
            }
        }

        public bool Equals(Sprite other)
        {
            return (this.X == other.X) && (this.Y == other.Y) && (this.Direction.Equals(other.Direction));
        }

        /// <summary>
        /// Compare this Sprite to a Point
        /// </summary>
        /// <param name="other">The Point object to compare this Sprite to</param>
        /// <returns><see langword="true"/>, if the Sprite and Point object have equal (X, Y) coordinates. False otherwise</returns>
        public bool Equals(Point other)
        {
            return (this.X == other.X) && (this.Y == other.Y);
        }

        public override string ToString()
        {
            return $"{X} {Y} {Direction}";
        }

        #endregion

        #region Helpers

        public enum TurnDirection
        {
            Left,
            Right
        };

        public class Heading
        {
            public const char North = 'N';
            public const char East = 'E';
            public const char South = 'S';
            public const char West = 'W';
        }

        #endregion
    }
}
