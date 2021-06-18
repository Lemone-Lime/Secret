using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Hasel;
using System;
using System.Collections.Generic;
using System.IO;

namespace Secret
{
    public class Secret : Engine
    {

        public Secret(int width, int height, string windowTitle, Color clearColour, bool fullscreen) : base(width, height, windowTitle, clearColour, fullscreen)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Scene = new CollisionsScene();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
