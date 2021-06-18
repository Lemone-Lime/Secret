using System;
using System.Collections.Generic;
using System.Text;

namespace Hasel
{
    public class Component
    {
        //Store which entity it is attached to.
        public bool Active { get; set; }
        public Entity Entity { get; private set; }

        public int TypeId
        {
            get { return Forge.ComponentTypes[GetType()]; }
        }
        public Component() : this(true)
        {
        }
        public Component(bool ACTIVE)
        {
            Active = ACTIVE;
        }
        internal virtual void Added(Entity ENTITY)
        {
            Entity = ENTITY;
            Prereq(Entity);
        }
        internal virtual void Removed(Entity ENTITY)
        {
            Entity = null;
        }
        internal virtual void Prereq(Entity ENTITY)
        {
            SoftReq(ENTITY);
        }
        internal virtual void SoftReq(Entity ENTITY)
        {

        }
    }
}
