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
    public class Dropdown : Button
    {
        public List<HInterface> Items;
        public bool Open;
        public Vector2 InitialDropOffset, DropDirection;

        public Dropdown(Vector2? OFFSET = null, HTexture TEXTURE = null, HText TEXT = null, Vector2? INITIALDROPOFFSET = null, Vector2? DROPDIRECTION = null) : base(OFFSET, TEXTURE, TEXT, null)
        {
            Items = new List<HInterface>();
            InitialDropOffset = INITIALDROPOFFSET ?? new Vector2(0, Dimensions.Y);
            DropDirection = DROPDIRECTION ?? new Vector2(0, 1);
        }

        public override void Update()
        {
            foreach (var item in Items)
            {
                item.InheritedOffset = Position;
                item.Update();
            }

            base.Update();

            Vector2 DropOffsetTarget = InitialDropOffset;
            if (Open)
            {
                foreach (var item in Items)
                {
                    item.DropOffset = Calc.Approach(item.DropOffset, DropOffsetTarget, Calc.Abs(DropOffsetTarget / 200));

                    item.Visible = true;

                    DropOffsetTarget += item.Dimensions * DropDirection;
                }
            }
            else
            {
                foreach (var item in Items)
                {
                    item.DropOffset = Calc.Approach(item.DropOffset, Vector2.Zero, Calc.Abs(DropOffsetTarget / 200));

                    if (item.DropOffset == Vector2.Zero) item.Visible = false;

                    DropOffsetTarget += item.Dimensions * DropDirection;
                }
            }
        }
        public override void Render()
        {
            base.Render();

            if (Visible)
            {
                foreach (var item in Items) item.Render();

                if (Open)
                    Limn.RenderCentered(Limn.Arrow, Position + new Vector2(Dimensions.X - 10, Dimensions.Y * 0.5f), Color.White, (float)Math.PI*1.5f, new Vector2(1f, 1f));
                else
                    Limn.RenderCentered(Limn.Arrow, Position + new Vector2(Dimensions.X - 10, Dimensions.Y * 0.5f), Color.White, (float)Math.PI*0.5f, new Vector2(1f, 1f));
            }
        }
        public override void Activate()
        {
            Open = !Open;
        }
        public virtual void Add(HInterface ITEM)
        {
            Items.Add(ITEM);
        }
        public virtual void Remove(HInterface ITEM)
        {
            Items.Remove(ITEM);
        }
    }
}
