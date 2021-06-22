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
    public class Window : Menu
    {
        public HInterface Bar;
        public Color? BarColour;
        public float BarWidth;

        public bool BarPressed = false;

        public Button Close;

        public Window(Vector2? OFFSET = null, HTexture TEXTURE = null, HText TEXT = null, float BARWIDTH = 24) : base(OFFSET, TEXTURE, TEXT)
        {
            Bar = new HInterface(Vector2.Zero, new HTexture((int)Dimensions.X, (int)BARWIDTH, BarColour ?? Color.DarkOrchid), null);
            Close = new Button(new Vector2(Dimensions.X - BARWIDTH, 0), new HTexture((int)BARWIDTH, (int)BARWIDTH, Color.Transparent), new HText("Hack", "x", new Vector2(BARWIDTH*0.25f, 0), Color.White, 0.2f), CloseWindow);
            Close.HighlightTarget = 0.2f;

            BarWidth = BARWIDTH;

            Items.Add(Bar);
            Items.Add(Close);    

            ShowArrow = false;
            Bar.ShowArrow = false;
            Close.ShowArrow = false;
        }
        public override void Update()
        {
            base.Update();

            if (Visible)
            {
                if (Bar.MouseInside)
                {
                    if (Scoop.Mouse.PressedLeft) BarPressed = true;
                }

                if (Scoop.Mouse.CheckLeft)
                {
                    if (BarPressed)
                    {
                        Offset += Scoop.Mouse.PositionDelta;
                    }
                }
                else
                {
                    Offset = Vector2.Clamp(Offset, Vector2.Zero, new Vector2(Engine.Width - Dimensions.X, Engine.Height - Dimensions.Y));
                    BarPressed = false;
                }
            }
        }
        public override void Render()
        {
            base.Render();

            if (Visible)
            {
                Limn.Render(Text, Vector2.Round(Position + TextLean));
            }
        }
        public override void Add(HInterface ITEM)
        {
            base.Add(ITEM);

            ITEM.Offset.Y += BarWidth;
        }
        public void CloseWindow() {
            Visible = false;
        }
    }
}
