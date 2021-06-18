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
    public class Circle : Shape
    {
        public Circle(Vector2 OFFSET, float RADIUS)
        {
            Type = PType.Circle;
            Set(OFFSET, RADIUS);
        }
        public void Set(Vector2 OFFSET, float RADIUS)
        {
            Radius = RADIUS;
            Offset = OFFSET;
            CalculateVolume();
        }
        public override void CalculateVolume()
        {
            Volume = (float)Math.PI * Radius * Radius;
        }
        public override void SetOrient(float RADIANS)
        {
        }
    }
}
