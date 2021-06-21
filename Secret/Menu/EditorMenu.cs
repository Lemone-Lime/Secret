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
#endregion

namespace Secret
{
    public class EditorMenu : Menu
    {
        Dropdown Edit;
        Toggle Shape;
        Button Exit, Restart, PlaceBox, PlaceCircle;
        Slider Density, Restitution;
        int size = 30;

        public EditorMenu()
        {
            Items = new List<HInterface>();
            Texture = new HTexture(Engine.Width, 24, new Color(10, 10, 10, 1));

            Edit = new Dropdown(Vector2.Zero, null, new HTexture(96, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Edit", new Vector2(10, 5)));
            Edit.HighlightTarget = 0.03f;
            Edit.TextLeanTarget = new Vector2(7, 0);

            Restart = new Button(new Vector2(96, 0), null, new HTexture(96, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Restart", new Vector2(10, 5)), RestartScene);
            Restart.HighlightTarget = 0.03f;
            Restart.TextLeanTarget = new Vector2(7, 0);

            Exit = new Button(new Vector2(192, 0), null, new HTexture(96, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Exit", new Vector2(10, 5)), Engine.Instance.Exit);
            Exit.HighlightTarget = 0.03f;
            Exit.TextLeanTarget = new Vector2(7, 0);

            Shape = new Toggle(Vector2.Zero, null, new HTexture(160, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Shape", new Vector2(10, 5)), new Vector2(160, 0));
            Shape.HighlightTarget = 0.03f;
            Shape.TextLeanTarget = new Vector2(7, 0);
            Edit.Add(Shape);

            PlaceBox = new Button(Vector2.Zero, null, new HTexture(96, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Box", new Vector2(10, 5)));
            PlaceBox.HighlightTarget = 0.03f;
            PlaceBox.TextLeanTarget = new Vector2(7, 0);
            PlaceBox.Visible = false;
            Shape.Add(PlaceBox);

            PlaceCircle = new Button(Vector2.Zero, null, new HTexture(96, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Circle", new Vector2(10, 5)));
            PlaceCircle.HighlightTarget = 0.03f;
            PlaceCircle.TextLeanTarget = new Vector2(7, 0);
            PlaceCircle.Visible = false;
            Shape.Add(PlaceCircle);

            Density = new Slider(Vector2.Zero, null, new HTexture(160, 32, new Color(10, 10, 10, 1)), new HText("Hack", "Density", new Vector2(10, 5)), new Vector2(30, 26), 100, 3);
            Density.LineColor = Color.White;
            Density.HighlightTarget = 0.03f;
            Density.TextLeanTarget = new Vector2(7, 0);
            Density.Visible = false;
            Density.CircleRadius = 3;
            Density.CircleThickness = 5;
            Edit.Add(Density);

            Restitution = new Slider(Vector2.Zero, null, new HTexture(160, 32, new Color(10, 10, 10, 1)), new HText("Hack", "Restitution", new Vector2(10, 5)), new Vector2(30, 26), 100, 3);
            Restitution.LineColor = Color.White;
            Restitution.HighlightTarget = 0.03f;
            Restitution.TextLeanTarget = new Vector2(7, 0);
            Restitution.Visible = false;
            Restitution.CircleRadius = 3;
            Restitution.CircleThickness = 5;
            Edit.Add(Restitution);

            Items.Add(Edit);
            Items.Add(Restart);
            Items.Add(Exit);
        }
        public override void Update()
        {
            base.Update();

            if (Scoop.Mouse.PressedLeft && !Engine.Instance.MouseHoveringUI)
            {
                if (Shape.Selected == PlaceCircle) PhysicsCircle();
                if (Shape.Selected == PlaceBox) PhysicsBox();
            }

            size = Math.Clamp(size + Math.Sign(Scoop.Mouse.WheelDelta), 10, 100);
        }

        public override void Render()
        {
            base.Render();

            Limn.HollowRectangle(Engine.Width - size, Engine.Height - size, size, size, Color.Yellow);
            Limn.Circle(new Vector2(Engine.Width - size / 2, Engine.Height - size / 2), size / 2, Color.Yellow, 5);
        }

        public void PhysicsCircle()
        {
            PhysicsEntity(new Circle(Vector2.Zero, 0.5f * size));
        }

        public void PhysicsBox()
        {
            PhysicsEntity(new Box(new Vector2(-size / 2, -size / 2), new Vector2(size, size)));
        }

        public void PhysicsEntity(Shape SHAPE)
        {
            PhysicsEntity physicsEntity = new PhysicsEntity();
            physicsEntity.Added(Engine.Scene);
            physicsEntity.Get<Transform>().Position = Scoop.Mouse.Position;
            physicsEntity.Get<Material>().Restitution = Calc.Lerp(0.1f, 0.9f, Restitution.Value);
            physicsEntity.Get<Material>().Density = Calc.Lerp(0.01f, 0.4f, Density.Value);

            physicsEntity.SetShape(SHAPE);
        }

        public void RestartScene()
        {
            Engine.Scene = new CollisionsScene();
        }
    }
}
