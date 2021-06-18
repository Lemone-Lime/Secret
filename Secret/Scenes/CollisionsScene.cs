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
        Entity Platform;

        int size = 30;

        public CollisionsScene() : base()
        {
            player = new PhysicsEntity();
            player.Added(this);
            player.SetShape(new Box(Vector2.Zero, new Vector2(30, 30)));
            Platform = new Entity();
            Platform.Added(this);
            Platform.AddComponent<Transform>();
            Platform.AddComponent<Collider>();
            Platform.AddComponent<Material>();
            Platform.Get<Collider>().MoldShape(new Box(new Vector2(0, 500), new Vector2(1024, 40)));
            Platform.AddTag("Ground");
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

            if (Scoop.Mouse.PressedLeft) PhysicsBox();
            if (Scoop.Mouse.PressedRight) PhysicsCircle();
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
                Limn.Render(shape, Color.White);
            }
            Limn.HollowRectangle(Engine.Width - size, Engine.Height - size, size, size, Color.Yellow);
            Limn.Circle(new Vector2(Engine.Width - size / 2, Engine.Height - size / 2), size / 2, Color.Yellow, 5);
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
            physicsEntity.SetShape(SHAPE);
        }   
    }
}