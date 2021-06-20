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
using System.Collections;
using System.Diagnostics;
#endregion

namespace Hasel
{
    public class Scene
    {
        public Forge Forge;
        public List<Entity> Entities { get; private set; }
        public Dictionary<int, List<Component>> Components { get; private set; }

        public Scene()
        {
            //Initialise Tags
            Forge = new Forge(this);
            Entities = Forge.Entities;
            Components = Forge.Components;
        }

        public virtual void Begin()
        {

        }
        public virtual void End()
        {
            
        }
        public virtual void Update()
        {

        }
        public virtual void FixedUpdate()
        {

        }
        public virtual void Render()
        {

        }
    }
}
