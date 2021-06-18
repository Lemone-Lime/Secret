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
    public class Image : Component
    {
        public Transform Transform { get; private set; }

        public PTexture Texture;

        public Color Color { get; set; }

        public bool Visible { get; set; }

        public Image() : this(true, true, null, null)
        {

        }
#nullable enable
        public Image(bool ACTIVE = true, bool VISIBLE = true, PTexture? TEXTURE = null, Color? COLOR = null) : base(ACTIVE)
        {

            Texture = TEXTURE ?? new PTexture();
#nullable disable
            Visible = VISIBLE;

            Color = COLOR ?? Color.White;
        }
        internal override void Prereq(Entity ENTITY)
        {
            Transform = ENTITY.Prereq<Transform>();
        }
    }
}
