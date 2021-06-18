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
            Get<Material>().Restitution = Calc.Random(0.1f, 0.9f);
            Get<Material>().Density = Calc.Random(0.05f, 0.2f);
        }
        public void SetShape(Shape SHAPE) {
            Get<Collider>().MoldShape(SHAPE);
            Get<Body>().CalculateMass(Get<Collider>(), Get<Material>());
        }
    }
}
