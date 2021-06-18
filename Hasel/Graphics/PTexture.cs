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
    public class PTexture
    {
        #region Variables
        private Vector2 origin;

        public Vector2 Origin
        {
            get { if (origin == null) return origin = new Vector2(Texture.Width / 2, Texture.Height/ 2); return origin; }
            private set { }
        }

        public Texture2D Texture { get; set; }

        public Rectangle Source { get; private set; }

        #endregion  
        public PTexture() : this("Missing", null)
        {
            
        }
        public PTexture(String PATH = "Missing", Rectangle? SOURCE = null) {

            Texture = Globals.Content.Load<Texture2D>(PATH);

            Source = SOURCE ?? new Rectangle(0, 0, Texture.Width, Texture.Height);
        }
        public PTexture(int WIDTH, int HEIGHT, Color COLOR)
        {
            Texture = new Texture2D(Engine.Instance.GraphicsDevice, WIDTH, HEIGHT);
            var colors = new Color[WIDTH * HEIGHT];
            for (int i = 0; i < WIDTH * HEIGHT; i++)
                colors[i] = COLOR;
            Texture.SetData<Color>(colors);

            Source = new Rectangle(0, 0, WIDTH, HEIGHT);

            Origin = Vector2.Zero;
        }
    }
}
