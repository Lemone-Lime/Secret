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
    //Handles rendering
    public static class Limn
    {
        public static BlendState BlendState;
        public static SamplerState SamplerState;

        public static HTexture Pixel;
        public static Rectangle Rect;
        public static SpriteFont Hack;
        public static HTexture Arrow;

        public static void Initialise()
        {
            BlendState = BlendState.AlphaBlend;
            SamplerState = SamplerState.PointClamp;
            Hack = Globals.Content.Load<SpriteFont>("Hack");
            Arrow = new HTexture("Arrow", null);

            Pixel = new HTexture(1, 1, Color.White);
        }
        public static void Begin()
        {
            Globals.Batch.Begin(SpriteSortMode.Deferred, BlendState, SamplerState, DepthStencilState.None);
        }
        public static void End()
        {
            Globals.Batch.End();
        }
        #region Render
        public static void Render(HTexture TEXTURE, Vector2 POSITION, Color COLOR, float ROTATION, Vector2 SCALE)
        {
            Globals.Batch.Draw(TEXTURE.Texture, POSITION, TEXTURE.Source, COLOR, ROTATION, Vector2.Zero, SCALE, SpriteEffects.None, 0);
        }
        public static void RenderCentered(HTexture TEXTURE, Vector2 POSITION, Color COLOR, float ROTATION, Vector2 SCALE)
        {
            Globals.Batch.Draw(TEXTURE.Texture, POSITION, TEXTURE.Source, COLOR, ROTATION, new Vector2(TEXTURE.Texture.Width, TEXTURE.Texture.Height) * 0.5f, SCALE, SpriteEffects.None, 0);
        }

        public static void Render(Image IMAGE)
        {
            if (IMAGE.Visible)
            {
                //Interpolated values
                Vector2 InterPosition = (IMAGE.Transform.PreviousPosition * Engine.Alpha) + (IMAGE.Transform.Position * (1.0f - Engine.Alpha));
                Vector2 InterScale = (IMAGE.Transform.PreviousScale * Engine.Alpha) + (IMAGE.Transform.Scale * (1.0f - Engine.Alpha));
                float InterRotation = (IMAGE.Transform.PreviousRotation * Engine.Alpha) + (IMAGE.Transform.Rotation * (1.0f - Engine.Alpha));

                IMAGE.Transform.PreviousPosition = IMAGE.Transform.Position;
                IMAGE.Transform.PreviousRotation = IMAGE.Transform.Rotation;
                IMAGE.Transform.PreviousScale = IMAGE.Transform.Scale;

                Render(IMAGE.Texture, InterPosition, IMAGE.Color, InterRotation, InterScale);
            }
        }

        public static void Render(Collider COLLIDER, Color COLOR)
        {
            Vector2 InterPosition = (COLLIDER.Transform.PreviousPosition * Engine.Alpha) + (COLLIDER.Transform.Position * (1.0f - Engine.Alpha));

            COLLIDER.Transform.PreviousPosition = COLLIDER.Transform.Position;

            switch (COLLIDER.Shape.Type)
            {
                case Shape.PType.Box:
                    if (COLLIDER.Entity.Contains<Body>())
                    {
                        HollowBox((Box)COLLIDER.Shape, COLOR);
                    }
                    else
                    {
                        Box((Box)COLLIDER.Shape, COLOR);
                    }
                    break;
                case Shape.PType.Circle:
                    Circle((Circle)COLLIDER.Shape, COLOR);
                    break;
                case Shape.PType.Poly:
                    Polygon((Polygon)COLLIDER.Shape, COLOR);
                    break;
            }
        }
        public static void Render(HText TEXT, Vector2 POSITION)
        {
            if (TEXT.Text != "")
                Globals.Batch.DrawString(TEXT.Font, TEXT.Text, POSITION + TEXT.Offset, TEXT.Color, 0.0f, Vector2.Zero, TEXT.Scale, new SpriteEffects(), 0.0f);
        }
        #endregion
        #region Point
        public static void Point(Vector2 AT, float SCALE, Color COLOR)
        {
            Globals.Batch.Draw(Pixel.Texture, AT, null, COLOR, 0.0f, Vector2.Zero, SCALE, new SpriteEffects(), 0);
        }
        #endregion
        #region Line 
        public static void Line(Vector2 START, Vector2 END, Color COLOR)
        {
            LineAngle(START, Calc.Angle(START, END), Vector2.Distance(START, END), COLOR);
        }
        public static void Line(Vector2 START, Vector2 END, float THICKNESS, Color COLOR)
        {
            LineAngle(START, Calc.Angle(START, END), Vector2.Distance(START, END), THICKNESS, COLOR);
        }
        public static void Line(Line LINE)
        {
            LineAngle(LINE.Start, LINE.Angle, LINE.Length, LINE.Color);
        }
        public static void LineAngle(Vector2 START, float ANGLE, float LENGTH, Color COLOR)
        {
            Globals.Batch.Draw(Pixel.Texture, START, Pixel.Source, COLOR, ANGLE, new Vector2(0, .5f), new Vector2(LENGTH, 1), SpriteEffects.None, 0);
        }
        public static void LineAngle(Vector2 START, float ANGLE, float LENGTH, float THICKNESS, Color COLOR)
        {
            Globals.Batch.Draw(Pixel.Texture, START, Pixel.Source, COLOR, ANGLE, new Vector2(0, .5f), new Vector2(LENGTH, THICKNESS), SpriteEffects.None, 0);
        }
        #endregion
        #region Circle
        public static void Circle(Vector2 POSITION, float RADIUS, Color COLOR, int RESOLUTION)
        {
            Vector2 last = Vector2.UnitX * RADIUS;
            Vector2 lastp = last.Perpendicular();

            for (int i = 1; i <= RESOLUTION; i++)
            {
                Vector2 at = Calc.AngleToVector(i * MathHelper.PiOver2 / RESOLUTION, RADIUS);
                Vector2 atp = at.Perpendicular();

                Line(POSITION + last, POSITION + at, COLOR);
                Line(POSITION - last, POSITION - at, COLOR);
                Line(POSITION + lastp, POSITION + atp, COLOR);
                Line(POSITION - lastp, POSITION - atp, COLOR);

                last = at;
                lastp = atp;    
            }
        }
        public static void Circle(Vector2 POSITION, float RADIUS, float THICKNESS, Color COLOR, int RESOLUTION)
        {
            Vector2 last = Vector2.UnitX * RADIUS;
            Vector2 lastp = last.Perpendicular();

            for (int i = 1; i <= RESOLUTION; i++)
            {
                Vector2 at = Calc.AngleToVector(i * MathHelper.PiOver2 / RESOLUTION, RADIUS);
                Vector2 atp = at.Perpendicular();

                Line(POSITION + last, POSITION + at, THICKNESS, COLOR);
                Line(POSITION - last, POSITION - at, THICKNESS,  COLOR);
                Line(POSITION + lastp, POSITION + atp, THICKNESS, COLOR);
                Line(POSITION - lastp, POSITION - atp, THICKNESS, COLOR);

                last = at;
                lastp = atp;
            }
        }
        #endregion
        #region Rectangle
        public static void Rectangle(float X, float Y, float WIDTH, float HEIGHT, Color COLOR)
        {
            Rect.X = (int)X;
            Rect.Y = (int)Y;
            Rect.Width = (int)WIDTH;
            Rect.Height = (int)HEIGHT;
            Globals.Batch.Draw(Pixel.Texture, Rect, COLOR);
        }
        public static void Rectangle(Vector2 POSITION, Vector2 DIMENSIONS, Color COLOR)
        {
            Rectangle(POSITION.X, POSITION.Y, DIMENSIONS.X, DIMENSIONS.Y, COLOR);
        }
        public static void Rectangle(Rectangle RECTANGLE, Color COLOR)
        {
            Rect = RECTANGLE;
            Globals.Batch.Draw(Pixel.Texture, Rect, COLOR);
        }
        #endregion
        #region Hollow Rectangle
        public static void HollowRectangle(float X, float Y, float WIDTH, float HEIGHT, Color COLOR)
        {
            Rect.X = (int)X;
            Rect.Y = (int)Y;
            Rect.Width = (int)WIDTH;
            Rect.Height = 1;

            Globals.Batch.Draw(Pixel.Texture, Rect, Pixel.Source, COLOR);

            Rect.Y += (int)HEIGHT - 1;

            Globals.Batch.Draw(Pixel.Texture, Rect, Pixel.Source, COLOR);

            Rect.Y -= (int)HEIGHT - 1;
            Rect.Width = 1;
            Rect.Height = (int)HEIGHT;

            Globals.Batch.Draw(Pixel.Texture, Rect, Pixel.Source, COLOR);

            Rect.X += (int)WIDTH - 1;

            Globals.Batch.Draw(Pixel.Texture, Rect, Pixel.Source, COLOR);
        }
        public static void HollowRectangle(Vector2 POSITION, Vector2 DIMENSIONS, Color COLOR)
        {
            HollowRectangle(POSITION.X, POSITION.Y, DIMENSIONS.X, DIMENSIONS.Y, COLOR);
        }
        public static void HollowRectangle(Rectangle RECTANGLE, Color COLOR)
        {
            HollowRectangle(RECTANGLE.X, RECTANGLE.Y, RECTANGLE.Width, RECTANGLE.Height, COLOR);
        }
        #endregion
        #region Polygon
        public static void Polygon(List<Vector2> POINTS, Color COLOR)
        {
            for (int i = 1; i < POINTS.Count; i++)
            {
                Line(POINTS[i-1], POINTS[i], COLOR);
            }
            Line(POINTS[POINTS.Count - 1], POINTS[0], COLOR);
        }
        #endregion
        #region Primitives
        public static void HollowBox(Box BOX, Color COLOR)
        {
            HollowRectangle(BOX.Position, BOX.Dimensions, COLOR);
        }
        public static void Box(Box BOX, Color COLOR)
        {
            Rectangle(BOX.Position, BOX.Dimensions, COLOR);
        }
        public static void Circle(Circle CIRCLE, Color COLOR)
        {
            Circle(CIRCLE.Position, CIRCLE.Radius, COLOR, Math.Max((int)CIRCLE.Radius/10, 5));
        }
        public static void Polygon(Polygon POLYGON, Color COLOR)
        {
            Polygon(POLYGON.Vertices, COLOR);
        }
        #endregion
        #region Text
        public static void Text(string TEXT, Vector2 POSITION, Color COLOR, float SCALE)
        {
            Globals.Batch.DrawString(Hack, TEXT, POSITION, COLOR, 0.0f, Vector2.Zero, SCALE, new SpriteEffects(), 0.0f);
        }
        public static void TextCentered(string TEXT, Vector2 POSITION, Color COLOR, float SCALE)
        {
            Globals.Batch.DrawString(Hack, TEXT, POSITION-Hack.MeasureString(TEXT)*0.5f, COLOR, 0.0f, Vector2.Zero, SCALE, new SpriteEffects(), 0.0f);
        }
        #endregion
    }
}
