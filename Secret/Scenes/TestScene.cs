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

using System.Collections;
using Hasel;
using System.Diagnostics;
using System.Collections.Specialized;
#endregion

namespace Secret
{
    public class TestScene : Scene
    {
        public Entity first;
        public Polygon DrawablePolygon;
        public List<Vector2> SetPoints;
        public Box TestBox;
        public Circle TestCircle;


        public Vector2 Self;

        public TestScene() : base()
        {
            first = new Entity();
            first.Added(this);
            first.AddComponent<Transform>();
            first.AddComponent<Image>();
            first.Get<Image>().Texture = new PTexture("Graphics\\bee monster");
            first.Get<Transform>().Scale = new Vector2(5, 5);

            TestBox = new Box(new Vector2(350, 280), new Vector2(140, 170));
            TestCircle = new Circle(new Vector2(100, 190), 80f);
            Self = Vector2.Zero;
            SetPoints = new List<Vector2>();
        }
        public override void Begin()
        {
            base.Begin();
        }
        public override void End()
        {
            base.End();
        }
        public override void Update()
        {
            base.Update();

            if (Scoop.Keyboard.Pressed(Keys.O)) SetPoints.Add(Self);
            if (Scoop.Keyboard.Pressed(Keys.P))
            {
                DrawablePolygon = new Polygon(SetPoints, Vector2.Zero);
                SetPoints.Clear();
            }

            Self += new Vector2(Scoop.Keyboard.AxisCheck(Keys.A, Keys.D) * 0.1f, Scoop.Keyboard.AxisCheck(Keys.W, Keys.S) * 0.1f);

            first.Get<Transform>().Position += new Vector2(0.1f, 0.1f);
        }
        public override void Render()
        {
            base.Render();

            if (DrawablePolygon != null)
            {
                Limn.Polygon(DrawablePolygon, Color.White);
                if (Atlas.PointPolygon(Self, DrawablePolygon)) Limn.Polygon(DrawablePolygon, Color.Yellow);
            }
            foreach (var point in SetPoints) Limn.Point(point, 4, Color.Lavender);
            Limn.Box(TestBox, Color.White);
            if (Atlas.PointBox(Self, TestBox)) Limn.Box(TestBox, Color.Yellow);
            Limn.Circle(TestCircle, Color.White);
            if (Atlas.PointCircle(Self, TestCircle)) Limn.Circle(TestCircle, Color.Yellow);
            Limn.Point(Self, 4, Color.Red);
        }
    }
}
