using System;
using System.Collections.Generic;
using System.Text;

namespace Hasel
{
    public class Material : Component
    {
        public float StaticFriction = 0.5f;
        public float KineticFriction = 0.36f;
        public float Restitution = 0.1f;
        public float Density = 0.4f;
    }
}
