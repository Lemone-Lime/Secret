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
    public class Checkbox : Button
    {
        public bool Checked = false;

        public Checkbox(Vector2? OFFSET = null, HTexture TEXTURE = null, HText TEXT = null) : base(OFFSET, TEXTURE, TEXT, null)
        {
            
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Render()
        {
            base.Render();

            if(Checked && Visible) Limn.Text("x", Position + new Vector2(Dimensions.X * 0.9f, Dimensions.Y * 0.5f - 8), Color.White, 0.12f);
        }
        public override void Activate()
        {
            Checked = !Checked;
        }
    }
}
