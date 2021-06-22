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
    public class CollisionsScene : Scene
    {
        PhysicsEntity player;
        StaticObject Platform;

        public CollisionsScene() : base()
        {
            player = new PhysicsEntity();
            player.Added(this);
            player.SetShape(new Box(Vector2.Zero, new Vector2(35, 60)));
            player.Get<Transform>().Position = new Vector2(497, 516);
            player.Get<Material>().Density = 0.1f;
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

            if (Scoop.Keyboard.Pressed(Keys.Z)) player.Get<Transform>().Position = Scoop.Mouse.Position;
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
        }
    }
}
