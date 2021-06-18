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
using Hasel;
using System.Collections.Specialized;
#endregion

namespace Hasel
{
    public class Entity
    {
        public Scene Scene { get; private set; }

        public int EntityId;
        internal int depth = 0;

        public int Depth
        {
            get { return depth; }
            set 
            { 
                if (depth != value) depth = value;
                if (Scene != null) Scene.Forge.SetActualDepth(this);
            }
        }
        internal double TrueDepth = 0;

        public Mask Mask;
        public TagMask Tag;

        public Entity()
        {
            Mask = new Mask();
            Tag = new TagMask(this);
        }
        public virtual void Added(Scene SCENE)
        {
            Scene = SCENE;

            Scene.Forge.EntityAdded(this);
        }
        public void AddTag(string TAGNAME)
        {
            Tag.Add(TAGNAME);
        }
        public void RemoveTag(string TAGNAME)
        {
            Tag.Remove(TAGNAME);
        }
        public bool TagFullCheck(string TAGNAME)
        {
            return Tag.FullCheck(TAGNAME);
        }
        public bool TagCheck(string TAGNAME)
        {
            return Tag.Check(TAGNAME);
        }
        public void AddComponent<T>() where T : Component, new()
        {
            if (!Contains<T>())
            {
                T component = new T();
                component.Added(this);
                Mask[component.TypeId] = true;
                Scene.Forge.ComponentAdded(component);
            }
        }
        public void RemoveComponent<T>() where T : Component
        {
            if (Contains<T>())
            {
                Component component = Get<T>();
                component.Removed(this);
                Mask[component.TypeId] = false;
                Scene.Forge.ComponentRemove(component);
            }
        }
        public T Get<T>() where T : Component
        {
            if(Contains<T>()) return Scene.Forge.Get<T>(this);
            return null;
        }
        public bool Contains<T>() where T : Component
        {
            return Mask[Scene.Forge.ComponentIndex<T>()];
        }
        public T Prereq<T>() where T : Component, new()
        {
            if (!Contains<T>()) {
                AddComponent<T>();
            }
            return Get<T>();
        }
        public T Softreq<T>() where T : Component
        {
            if (Contains<T>()) return Get<T>();
            return null;
        }
        public void Remove()
        {
            Scene.Forge.EntityRemove(this);
        }

        public static Comparison<Entity> CompareDepth = (a, b) => { return Math.Sign(b.TrueDepth - a.TrueDepth); };
    }
}
