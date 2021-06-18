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
#endregion

namespace Hasel
{
    public class Body : Component
    {
        public Vector2 Force = Vector2.Zero;
        public float Torque = 0.0f;

        public float Mass = 0;
        public float InvMass = 0;
        public float Inertia = 0;
        public float InvInertia = 0;

        public Body() : this(true)
        {
            
        }

        public Body(bool ACTIVE) : base(ACTIVE)
        {

        }
        public void CalculateMass(Collider COLLIDER, Material MATERIAL)
        {
            Mass = COLLIDER.Shape.Volume * MATERIAL.Density;
            if (Mass == 0)
                InvMass = 0;
            else
                InvMass = 1 / Mass;
        }
    }
}
