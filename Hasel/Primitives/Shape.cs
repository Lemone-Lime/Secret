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
    public abstract class Shape
    {
        public enum PType { Circle, Box, Poly, Undefined }
        public PType Type = PType.Undefined;
        public Vector2 Position { get { if(Transform != null) return Transform.Position + Offset; return Offset; } }
        public Vector2 Offset;
        public float Radius;
        public float Volume = 0;
        public bool OnGround = false;

        public virtual float Top { get { return Position.Y; } }
        public virtual float Bottom { get { return Position.Y + Radius * 2; } }
        public virtual float Left { get { return Position.X; } }
        public virtual float Right { get { return Position.X + Radius * 2; } }

        public virtual Vector2 Center{get { return new Vector2(Position.X + Radius, Position.Y+Radius); }}
        public Transform Transform { get; set; }

        public abstract void CalculateVolume();
        public abstract void SetOrient(float RADIANS);
    }   
}
