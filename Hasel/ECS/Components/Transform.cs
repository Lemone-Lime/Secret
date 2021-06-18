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
    public class Transform : Component
    {
        private Vector2 position;
        private Vector2 scale;
        private float rotation;

        public float Orientation;

        public Vector2 Position
        {
            get { return position; }
            set { if (!FreezePosition) position = value; }
        }
        public Vector2 PreviousPosition;
        public Vector2 Scale
        {
            get { return scale; }
            set { if (!FreezeScale) scale = value; }
        }
        public Vector2 PreviousScale;
        public float Rotation
        {
            get { return rotation; }
            set { if (!FreezeRotation) rotation = value; }
        }
        public float PreviousRotation;

        public bool FreezePosition = false;
        public bool FreezeRotation = false;
        public bool FreezeScale = false;

        public Transform() : this(true, null, null, 0)
        {

        }

        public Transform(bool ACTIVE = true, Vector2? POSITION = null, Vector2? SCALE = null, float ROTATION = 0) : base(ACTIVE)
        {
            Position = POSITION ?? Vector2.Zero;
            Scale = SCALE ?? Vector2.One;
            Rotation = ROTATION;
        }
    }
}
