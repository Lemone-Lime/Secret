﻿#region Includes
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
    public class CollisionsScene : Scene
    {
        PhysicsEntity player;
        StaticObject Platform;
        HInterface Bar;
        Dropdown Edit;
        Button Exit, Restart;
        Checkbox PlaceBox, PlaceCircle;
        Slider Density, Restitution;

        int size = 30;

        public CollisionsScene() : base()
        {
            Bar = new HInterface(new Vector2(192, 0), null, new HTexture(Engine.Width - 192, 24, new Color(20, 20, 20, 1)), null);

            Edit = new Dropdown(Vector2.Zero, null, new HTexture(96, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Edit", new Vector2(10, 5)));
            Edit.HighlightTarget = 0.03f;
            Edit.TextLeanTarget = new Vector2(7, 0);

            Restart = new Button(new Vector2(96, 0), null, new HTexture(96, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Restart", new Vector2(10, 5)), RestartScene);
            Restart.HighlightTarget = 0.03f;
            Restart.TextLeanTarget = new Vector2(7, 0);

            Exit = new Button(new Vector2(198, 0), null, new HTexture(96, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Exit", new Vector2(10, 5)), Engine.Instance.Exit);
            Exit.HighlightTarget = 0.03f;
            Exit.TextLeanTarget = new Vector2(7, 0);

            PlaceBox = new Checkbox(Vector2.Zero, null, new HTexture(160, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Place Box", new Vector2(10, 5)));
            PlaceBox.HighlightTarget = 0.03f;
            PlaceBox.TextLeanTarget = new Vector2(7, 0);
            PlaceBox.Visible = false;
            Edit.Add(PlaceBox);

            PlaceCircle = new Checkbox(Vector2.Zero, null, new HTexture(160, 24, new Color(10, 10, 10, 1)), new HText("Hack", "Place Circle", new Vector2(10, 5)));
            PlaceCircle.HighlightTarget = 0.03f;
            PlaceCircle.TextLeanTarget = new Vector2(7, 0);
            PlaceCircle.Visible = false;
            Edit.Add(PlaceCircle);

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

            player = new PhysicsEntity();
            player.Added(this);
            player.SetShape(new Box(new Vector2(497, 516), new Vector2(30, 30)));
            player.Get<Material>().Restitution = 0.5f;


            Platform = new StaticObject();
            Platform.Added(this);
            Platform.SetShape(new Box(new Vector2(100, 546), new Vector2(824, 30)));
            Platform.Get<Material>().Restitution = 0.7f;
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

            Edit.Update();
            Restart.Update();
            Exit.Update();

            if (Scoop.Mouse.PressedLeft && !Engine.Instance.MouseHoveringUI)
            {
                if (PlaceCircle.Checked) PhysicsCircle();
                if (PlaceBox.Checked) PhysicsBox();
            }

            if (Scoop.Keyboard.Pressed(Keys.Z)) player.Get<Transform>().Position = Scoop.Mouse.Position;

            size = Math.Clamp(size + Math.Sign(Scoop.Mouse.WheelDelta), 10, 100);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            player.Get<Body>().Force += new Vector2(Scoop.Keyboard.AxisCheck(Keys.A, Keys.D) * 50000, Scoop.Keyboard.AxisCheck(Keys.W, Keys.S) * 50000);
        }
        public override void Render()
        {
            base.Render();

            foreach (var shape in Forge.ComponentList<Collider>())
            {
                Limn.Render(shape, Color.Lerp(Color.Transparent, Color.Lerp(Color.Blue, Color.Red, shape.Entity.Get<Material>().Restitution), 0.6f+shape.Entity.Get<Material>().Density));
            }

            Limn.HollowRectangle(Engine.Width - size, Engine.Height - size, size, size, Color.Yellow);
            Limn.Circle(new Vector2(Engine.Width - size / 2, Engine.Height - size / 2), size / 2, Color.Yellow, 5);

            Bar.Render();
            Edit.Render();
            Restart.Render();
            Exit.Render();
        }

        public void PhysicsCircle()
        {
            PhysicsEntity(new Circle(Vector2.Zero, 0.5f*size));
        }

        public void PhysicsBox()
        {
            PhysicsEntity(new Box(new Vector2(-size/2, -size/2), new Vector2(size, size)));
        }

        public void PhysicsEntity(Shape SHAPE)
        {
            PhysicsEntity physicsEntity = new PhysicsEntity();
            physicsEntity.Added(this);
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
