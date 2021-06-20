using System;
using System.Collections.Generic;
using System.Text;
using Hasel;

namespace Secret
{
    public class StaticObject : Entity
    {
        public StaticObject()
        {

        }
        public override void Added(Scene SCENE)
        {
            base.Added(SCENE);

            AddComponent<Transform>();
            AddComponent<Collider>();
            AddComponent<Material>();
        }
        public void SetShape(Shape SHAPE)
        {
            Get<Collider>().MoldShape(SHAPE);
        }
    }
}
