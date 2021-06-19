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
    public class HText
    {
        public SpriteFont Font;
        public string Text;

        public Color Color;
        public Vector2 Offset;

        public Vector2 Dimensions;

        public float Scale;

        public HText(string FONTPATH = "Hack", string TEXT = "", Vector2? OFFSET = null, Color? COLOR = null, float SCALE = 0.12f)
        {
            Font = Globals.Content.Load<SpriteFont>(FONTPATH);
            Text = TEXT;
            Offset = OFFSET ?? Vector2.Zero;
            Color = COLOR ?? Color.White;
            Scale = SCALE;
            Dimensions = Font.MeasureString(TEXT) * Scale;
        }
    }
}
