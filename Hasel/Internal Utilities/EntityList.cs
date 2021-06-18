using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Hasel
{
    public class EntityList : IEnumerable<Entity>, IEnumerable
    {
        private List<Entity> Entities;
        public bool Unsorted;

        public int Count { get { return Entities.Count; } }

        public Entity this[int INDEX] {
            get { if (INDEX < 0 || INDEX >= Count) throw new Exception("Index out of range"); return Entities[INDEX]; }
        }

        public EntityList()
        {
            Entities = new List<Entity>();
        }

        public void UpdateList()
        {
            if (Unsorted)
            {
                Entities.Sort(CompareDepth);
                Unsorted = false;
            }
        }

        public void Add(Entity ENTITY)
        {
            if(!Entities.Contains(ENTITY))Entities.Add(ENTITY);
        }
        public void Remove(Entity ENTITY)
        {
            if (Entities.Contains(ENTITY)) Entities.Remove(ENTITY);
        }
        public IEnumerator<Entity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        public EntityList Get(Mask MASK)
        {
            EntityList list = new EntityList();
            foreach (var entity in Entities)
            {
                if (entity.Mask.FullCheck(MASK.Value)) list.Add(entity);
            }
            return list;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public static Comparison<Entity> CompareDepth = (a, b) => { return Math.Sign(b.TrueDepth - a.TrueDepth); };
    }
}
