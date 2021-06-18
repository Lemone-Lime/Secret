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
    public class Dynamo : Component
    {
        //Linear Components
        public Vector2 Velocity = Vector2.Zero;
        public float GravityScale = 1f;

        //Rotational Components
        public float AngularVelocity = 0.0f;

        public Dynamo() : this(true)
        {

        }

        public Dynamo(bool ACTIVE) : base(ACTIVE)
        {

        }
        internal override void Added(Entity ENTITY)
        {
            base.Added(ENTITY);
        }
    }
}
