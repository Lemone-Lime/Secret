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
        //Essential values
        public Vector2 Position, Dimensions;
        public HTexture Texture;

        public Vector2 Offset = Vector2.Zero;
        public Vector2 DropOffset = Vector2.Zero;
        public Vector2 InheritedOffset = Vector2.Zero;


        //Highlight
        public float Highlight = 0.0f;
        public float HighlightTarget = 0.0f;

        //Text and Textlean
        public HText Text;
        public Vector2 TextLean;
        public Vector2 TextLeanTarget;

        //Arrow Properties
        public float ArrowScale = 0.0f;
        public float ArrowAlpha = 0.0f;

        public bool ShowArrow = true;
        public bool MouseInside = false;
        public bool Visible = true;


        public HInterface(Vector2? OFFSET = null, HTexture TEXTURE = null, HText TEXT = null)
        {
            Offset = OFFSET ?? Vector2.Zero;
            Texture = TEXTURE;

            if (Texture == null)
                Dimensions = TEXT.Dimensions;
            else
                Dimensions = Texture.Dimensions;

            Text = TEXT ?? new HText();
        }
        public virtual void Update()
        {
            Position = Offset + DropOffset + InheritedOffset;

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
                if (Texture != null)
                {
                    Limn.Render(Texture, Position, Color.White, 0.0f, Vector2.One);
                    Limn.Rectangle(Position, Dimensions, Color.Lerp(Color.Transparent, Color.White, Highlight));
                }

                Limn.Render(Text, Vector2.Round(Position + TextLean));

                if (ShowArrow)
                {
                    Limn.RenderCentered(Limn.Arrow, Position + new Vector2(8, Dimensions.Y * 0.5f), Color.Lerp(Color.Transparent, Color.White, ArrowAlpha), 0.0f, new Vector2(ArrowScale, ArrowScale));
                }

            }
        }

        public bool InDimensions() 
        {
            if (Scoop.Mouse.Position.X < Position.X || Scoop.Mouse.Position.X > Position.X + Dimensions.X) return false;
            if (Scoop.Mouse.Position.Y < Position.Y || Scoop.Mouse.Position.Y > Position.Y + Dimensions.Y) return false;
            return true;
        }
    }
}
