using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hasel
{
    public class Collider : Component
    {
        public Shape Shape;
        public Transform Transform { get; private set; }
        public Shape.PType Type { get; private set; }

        public Vector2 Position { get { return Transform.Position; } set { Transform.Position = value; } }

        public Collider() : this(true)
        {

        }
        public Collider(bool ACTIVE) : base(ACTIVE)
        {

        }
        public void MoldShape(Shape SHAPE)
        {
            Shape = SHAPE;
            Shape.Transform = Transform;
            Type = SHAPE.Type;
        }
        internal override void Added(Entity ENTITY)
        {
            base.Added(ENTITY);
        }
        internal override void Prereq(Entity ENTITY)
        {
            Transform = Entity.Prereq<Transform>();
        }
    }
}
