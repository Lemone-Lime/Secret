using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hasel
{
    public class Line
    {
        public Vector2 Start, End;
        public Color Color;
        public float Thickness;
        public float Length { get { return Vector2.Distance(Start, End); } }
        public float Angle { get { return Calc.Angle(Start, End); } }

        public Line(Vector2? START = null, Vector2? END = null, Color? COLOR = null, float THICKNESS = 1f)
        {
            Start = START ?? Vector2.Zero;
            End = END ?? Vector2.One;
            Color = COLOR ?? Color.White;
            Thickness = THICKNESS;
        }
    }
}
