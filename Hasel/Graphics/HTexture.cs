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
    public class HTexture
    {
        #region Variables
        public Texture2D Texture { get; set; }
        public Vector2 Dimensions { get { return new Vector2(Texture.Width, Texture.Height); } }
        public Rectangle Source { get; private set; }

        #endregion  
        public HTexture() : this("Missing", null)
        {
            
        }
        public HTexture(String PATH = "Missing", Rectangle? SOURCE = null) {

            Texture = Globals.Content.Load<Texture2D>(PATH);

            Source = SOURCE ?? new Rectangle(0, 0, Texture.Width, Texture.Height);
        }
        public HTexture(int WIDTH, int HEIGHT, Color COLOR)
        {
            Texture = new Texture2D(Engine.Instance.GraphicsDevice, WIDTH, HEIGHT);
            var colors = new Color[WIDTH * HEIGHT];
            for (int i = 0; i < WIDTH * HEIGHT; i++)
                colors[i] = COLOR;
            Texture.SetData<Color>(colors);

            Source = new Rectangle(0, 0, WIDTH, HEIGHT);
        }
    }
}
