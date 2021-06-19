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
    public class HInterface
    {
        public Vector2 Position, Dimensions;
        public HTexture Texture;

        public float Highlight = 0.0f;
        public float HighlightTarget = 0.0f;

        public HText Text;
        public Vector2 TextLean;
        public Vector2 TextLeanTarget;

        public bool MouseInside = false;

        public Vector2 DropOffset = Vector2.Zero;

        public bool Visible = true;

        public float ArrowScale = 0.0f;
        public float ArrowAlpha = 0.0f;

        public HInterface(Vector2? POSITION = null, Vector2? DIMENSIONS = null, HTexture TEXTURE = null, HText TEXT = null)
        {
            Position = POSITION ?? Vector2.Zero;
            Texture = TEXTURE ?? new HTexture();
            Dimensions = DIMENSIONS ?? new Vector2(Texture.Texture.Width, Texture.Texture.Height);

            Text = TEXT ?? new HText();
        }
        public virtual void Update()
        {
            if (InDimensions() && Visible)
            {
                Engine.Instance.MouseHoveringUI = true;

                Highlight = Calc.Approach(Highlight, HighlightTarget, HighlightTarget / 200);
                TextLean = Calc.Approach(TextLean, TextLeanTarget, TextLeanTarget / 200);

                ArrowScale = Calc.Approach(ArrowScale, 0.5f, 0.0025f);
                ArrowAlpha = Calc.Approach(ArrowAlpha, 1, 0.005f);

                MouseInside = true;
            }
            else
            {
                Highlight = Calc.Approach(Highlight, 0, HighlightTarget / 200);
                TextLean = Calc.Approach(TextLean, Vector2.Zero, TextLeanTarget / 200);

                ArrowScale = Calc.Approach(ArrowScale, 0, 0.0025f);
                ArrowAlpha = Calc.Approach(ArrowAlpha, 0, 0.005f);

                MouseInside = false;
            }
        }

        public virtual void Render() 
        {
            if (Visible)
            {
                Limn.Render(Texture, Position + DropOffset, Color.White, 0.0f, Vector2.One);
                Limn.Rectangle(Position + DropOffset, Dimensions, Color.Lerp(Color.Transparent, Color.White, Highlight));

                Limn.RenderCentered(Limn.Arrow, Position + DropOffset + new Vector2(8, Dimensions.Y*0.5f), Color.Lerp(Color.Transparent, Color.White, ArrowAlpha), 0.0f, new Vector2(ArrowScale, ArrowScale));

                Limn.Render(Text, Vector2.Round(Position + TextLean + DropOffset));
            }
        }

        public bool InDimensions() 
        {
            if (Scoop.Mouse.Position.X < Position.X + DropOffset.X || Scoop.Mouse.Position.X > Position.X + DropOffset.X + Dimensions.X) return false;
            if (Scoop.Mouse.Position.Y < Position.Y + DropOffset.Y || Scoop.Mouse.Position.Y > Position.Y + DropOffset.Y + Dimensions.Y) return false;
            return true;
        }
    }
}
