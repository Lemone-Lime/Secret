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
    public class Menu : HInterface
    {
        public List<HInterface> Items;

        public Menu(Vector2? OFFSET = null, Vector2? DIMENSIONS = null, HTexture TEXTURE = null, HText TEXT = null) : base(OFFSET, DIMENSIONS, TEXTURE, TEXT)
        {
            
        }
        public override void Update()
        {
            foreach (var item in Items)
            {
                item.InheritedOffset = Position;
                item.Update();
            }

            base.Update();            
        }
        public override void Render()
        {
            base.Render();

            if (Visible) foreach (var item in Items) item.Render();
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
