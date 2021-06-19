﻿#region Includes
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
    public class Button : HInterface
    {
        public bool Pressed;
        public Globals.HAction Action;

        public Button(Vector2? POSITION = null, Vector2? DIMENSIONS = null, HTexture TEXTURE = null, HText TEXT = null, Globals.HAction ACTION = null) : base(POSITION, DIMENSIONS, TEXTURE, TEXT)
        {
            Action = ACTION ?? Globals.Boop;
        }
        public override void Update()
        {
            base.Update();

            if (MouseInside)
            {
                if (Scoop.Mouse.PressedLeft)
                {
                    Pressed = true;
                }
                else if (Scoop.Mouse.ReleasedLeftButton && Pressed)
                {
                    Pressed = false;
                    Activate();
                }
            }
            else
            {
                if (Scoop.Mouse.ReleasedLeftButton) Pressed = false;
            }
        }
        public override void Render()
        {
            base.Render();
        }
        public virtual void Activate()
        {
            Action.Invoke();
        }
    }
}