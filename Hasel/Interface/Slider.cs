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
    public class Slider : HInterface
    {
        public float Value = 0.5f;
        public Color HighlightCircleColor;
        public float HighlightCircleRadius;
        public int CircleResolution = 3;

        public Color CircleColor = Color.DeepSkyBlue;
        public float CircleRadius, CircleThickness;
        public Vector2 CircleOffset;

        public Color LineColor = Color.White;
        public Vector2 LineOffset;
        public float LineLength, LineThickness;

        public bool Pressed = false;

        public Slider(Vector2? OFFSET = null, HTexture TEXTURE = null, HText TEXT = null, Vector2? LINEOFFSET = null, float LINELENGTH = 10, float LINETHICKNESS = 1) : base(OFFSET, TEXTURE, TEXT)
        {
            LineOffset = LINEOFFSET ?? Vector2.Zero;
            LineLength = LINELENGTH;
            LineThickness = LINETHICKNESS;

            CircleThickness = LineThickness;
            CircleRadius = LineThickness * 2;
            CircleOffset = new Vector2(LineLength * Value, 0);
        }
        public override void Update()
        {
            base.Update();

            if (Atlas.PointCircle(Scoop.Mouse.Position, new Circle(Position + LineOffset + CircleOffset, CircleRadius + CircleThickness * 0.5f)))
            {
                if (Scoop.Mouse.PressedLeft)
                {
                    Pressed = true;
                }
            }
            if (Pressed && Scoop.Mouse.CheckLeft)
            {
                CircleOffset = new Vector2(Math.Clamp(Scoop.Mouse.X - LineOffset.X - Position.X, 0, LineLength), 0);
                Value = CircleOffset.X / LineLength;
            }
            if (Scoop.Mouse.ReleasedLeftButton) Pressed = false;
        }
        public override void Render()
        {
            base.Render();

            if (Visible)
            {
                Limn.LineAngle(Position + LineOffset, 0.0f, LineLength, LineThickness, LineColor);
                Limn.Circle(Position + LineOffset + CircleOffset, CircleRadius, CircleThickness, CircleColor, CircleResolution);
            }
        }
    }
}
