#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace Hasel
{
    public class Box : Shape
    {
        public Vector2 Dimensions;
        public override float Left { get { return Position.X; } }
        public override float Right { get { return Position.X + Dimensions.X; } }
        public override float Top { get { return Position.Y; } }
        public override float Bottom { get { return Position.Y + Dimensions.Y; } }
        public Vector2 Min { get { return Position; } }
        public Vector2 Max { get { return Position + Dimensions; } }
        public Vector2 HalfDimensions { get { return (Max-Min)/2; } }
        public override Vector2 Center { get { return Position + HalfDimensions; } }

        public Box(Vector2 OFFSET, Vector2 DIMENSIONS)
        {
            Set(OFFSET, DIMENSIONS);
            Type = PType.Box;
        }
        public Box(float X, float Y, float WIDTH, float HEIGHT)
        {
            Set(new Vector2(X, Y), new Vector2(WIDTH, HEIGHT));
            Type = PType.Box;
        }
        public void Set(Vector2 OFFSET, Vector2 DIMENSIONS)
        {
            Offset = OFFSET;
            Dimensions = DIMENSIONS;
            CalculateVolume();
        }
        public override void CalculateVolume()
        {
            Volume = Dimensions.X * Dimensions.Y;
        }
        public override void SetOrient(float RADIANS)
        {
            
        }
    }
}
