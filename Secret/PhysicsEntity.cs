using System;
using System.Collections.Generic;
using System.Text;
using Hasel;
using Microsoft.Xna.Framework;

namespace Secret
{
    public class PhysicsEntity : Entity
    {
        public PhysicsEntity()
        {

        }
        public override void Added(Scene SCENE)
        {
            base.Added(SCENE);

            AddComponent<Transform>();
            AddComponent<Collider>();
            AddComponent<Dynamo>();
            AddComponent<Material>();
            AddComponent<Body>();
        }
        public void SetShape(Shape SHAPE) {
            Get<Collider>().MoldShape(SHAPE);
            Get<Body>().CalculateMass(Get<Collider>(), Get<Material>());
        }
    }
}
