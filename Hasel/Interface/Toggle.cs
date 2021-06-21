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
    public class Toggle : Dropdown
    {
        public Button Selected;
        public List<Button> Buttons;
        

        public Toggle(Vector2? OFFSET = null, Vector2? DIMENSIONS = null, HTexture TEXTURE = null, HText TEXT = null, Vector2? INITIALDROPOFFSET = null) : base(OFFSET, DIMENSIONS, TEXTURE, TEXT, INITIALDROPOFFSET)
        {
            Buttons = new List<Button>();
        }

        public override void Update()
        {
            base.Update();

            if (Open) {
                foreach (var Button in Buttons)
                {
                    if (Button.Activated)
                    {
                        if (Button != Selected)
                        {
                            Selected = Button;
                        }
                        else
                        {
                            Selected = null;
                        }

                    }
                }
            }
        }

        public override void Render()
        {
            base.Render();

            if (Selected != null)
            {
                if(Selected.Visible && Visible) Limn.Text("x", Selected.Position + new Vector2(Selected.Dimensions.X * 0.9f, Selected.Dimensions.Y * 0.5f - 8), Color.White, 0.12f);
            }
        }
        public override void Add(HInterface ITEM)
        {
            base.Add(ITEM);

            if (ITEM.GetType() == typeof(Button)) {
                Buttons.Add((Button) ITEM);
            }
        }
    }
}
