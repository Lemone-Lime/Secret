using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace Hasel
{
    public class TagMask : Mask
    {
        public Entity Entity { get; private set; }
        public override int Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    if (Entity != null)
                    {
                        for (int i = 0; i < Tag.TotalTags; i++)
                        {
                            int check = 1 << i;
                            //Whether what it is being set to has this bit
                            bool add = (check & value) != 0; ;
                            //Whether the tag already contains this bit;
                            bool has = (check & this.value) != 0;
                            if (has != add) //check if changing
                            {
                                if (add) // add to scene
                                    Entity.Scene.Forge.TagLists[i].Add(Entity);
                                else
                                    Entity.Scene.Forge.TagLists[i].Remove(Entity);
                            }
                        }
                    }
                    this.value = value;
                }
            }
        }
        public TagMask(Entity ENTITY) : base()
        {
            Entity = ENTITY;
        }
        public void Add(string TAGNAME)
        {
            Add(Tag.Get(TAGNAME).Value);
        }
        public void Remove(string TAGNAME)
        {
            Remove(Tag.Get(TAGNAME).Value);
        }
        public bool Check(string TAGNAME)
        {
            return Check(Tag.Get(TAGNAME).Value);
        }
        public bool FullCheck(string TAGNAME)
        {
            return FullCheck(Tag.Get(TAGNAME).Value);
        }
    }
}
