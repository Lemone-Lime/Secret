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
    public class Dropdown : Button
    {
        public List<HInterface> Items;
        public bool Open;
        public Vector2 InitialDropOffset;

        public Dropdown(Vector2? POSITION = null, Vector2? DIMENSIONS = null, HTexture TEXTURE = null, HText TEXT = null, Vector2? INITIALDROPOFFSET = null) : base(POSITION, DIMENSIONS, TEXTURE, TEXT, null)
        {
            Items = new List<HInterface>();
            InitialDropOffset = INITIALDROPOFFSET ?? new Vector2(0, Dimensions.Y);
        }

        public override void Update()
        {
            foreach (var item in Items) item.Update();

            base.Update();

            Vector2 DropOffsetTarget = InitialDropOffset;
            if (Open)
            {
                foreach (var item in Items)
                {
                    item.DropOffset.X = InitialDropOffset.X;
                    item.DropOffset.Y = Calc.Approach(item.DropOffset.Y, DropOffsetTarget.Y, DropOffsetTarget.Y / 100);

                    item.Visible = true;

                    DropOffsetTarget.Y += item.Dimensions.Y;
                }
            }
            else
            {
                foreach (var item in Items)
                {
                    item.DropOffset.X = InitialDropOffset.X;
                    item.DropOffset.Y = Calc.Approach(item.DropOffset.Y, 0, DropOffsetTarget.Y / 100);
                    if (item.DropOffset.Y == 0) item.Visible = false;

                    DropOffsetTarget.Y += item.Dimensions.Y;
                }
            }
        }
        public override void Render()
        {
            foreach (var item in Items) item.Render();

            base.Render();
        }
        public override void Activate()
        {
            Open = !Open;
        }
        public void Add(HInterface ITEM)
        {
            Items.Add(ITEM);
        }
        public void Remove(HInterface ITEM)
        {
            Items.Remove(ITEM);
        }
    }
}
