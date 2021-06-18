#region Includes
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
#endregion

namespace Hasel
{
    public class Forge
    {
        internal static Dictionary<Type, int> ComponentTypes;
        public static int ComponentCount = 0;
        public const int ComponentTypeCap = 32;

        internal Dictionary<int, List<Component>> Components;

        private bool AreAnyUnsorted = false;
        private bool[] Unsorted;


        internal List<Entity>[] TagLists;
        private Dictionary<int, double> TrueDepths;

        internal List<Entity> Entities;
        private List<int> FreeIds;
        private int previousId = 0;

        public Scene Scene { get; private set; }

        public static void Initialise()
        {
            ComponentTypes = new Dictionary<Type, int>(ComponentTypeCap);

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeof(Component).IsAssignableFrom(type) && type != typeof(Component))
                {
                    ComponentTypes.Add(type, ComponentCount++);
                }
            }
        }

        public Forge(Scene SCENE)
        {
            Entities = new List<Entity>();
            Components = new Dictionary<int, List<Component>>();

            TagLists = new List<Entity>[Tag.TotalTags];
            for (var i = 0; i < TagLists.Length; i++) { TagLists[i] = new List<Entity>();}

            Unsorted = new bool[Tag.TotalTags];
            TrueDepths = new Dictionary<int, double>();

            foreach (var type in ComponentTypes)
            {
                Components.Add(type.Value, new List<Component>());
            }

            FreeIds = new List<int>();
            Scene = SCENE;
        }
        public void EntityAdded(Entity ENTITY)
        {
            ENTITY.EntityId = GenerateId();
            Entities.Insert(ENTITY.EntityId, ENTITY);
            AddToTagLists(ENTITY);

            if (ENTITY.EntityId + 1 == previousId) {
                foreach (var list in Components.Values)
                {
                    list.Add(new Component());
                }
            }
            
        }
        public void EntityRemove(Entity ENTITY)
        {
            foreach (var keyValue in ComponentTypes)
            {
                int typeId = ComponentTypes[keyValue.Key];
                if (ENTITY.Mask[typeId])
                {
                    Components[typeId].RemoveAt(ENTITY.EntityId);
                    ENTITY.Mask[typeId] = false;
                }
            }
            Entities.Remove(ENTITY);
            FreeIds.Add(ENTITY.EntityId);
        }
        public Entity GetEntity(int INDEX)
        {
            return Entities[INDEX];
        }
        public void ComponentAdded(Component COMPONENT)
        {
            Components[COMPONENT.TypeId][COMPONENT.Entity.EntityId] = COMPONENT;
            //UpdateSoftReq(COMPONENT.Entity);
        }
        public void ComponentRemove(Component COMPONENT)
        {
            Components[COMPONENT.TypeId].Remove(COMPONENT);
        }
        public int ComponentIndex<T>() where T : Component
        {
            return ComponentTypes[typeof(T)];
        }
        public List<T> ComponentList<T>() where T : Component
        {
            List<T> list = new List<T>();
            foreach (var element in Components[ComponentIndex<T>()])
            {
                if(typeof(T).IsAssignableFrom(element.GetType()))list.Add((T)element);
            }
            return list;
        }
        public List<Component> GetComponentsByEntity(Entity ENTITY)
        {
            List<Component> temp = new List<Component>();
            foreach (var component in Components.Values)
            {
                if (component[ENTITY.EntityId] != null)
                {
                    temp.Add(component[ENTITY.EntityId]);
                }
            }
            return temp;
        }
        public List<Entity> GetEntityByMask(List<Entity> ENTITIES, Mask MASK)
        {
            List<Entity> temp = new List<Entity>();
            foreach (var entity in ENTITIES)
            {
                if (entity.Mask.FullCheck(MASK.Value)) temp.Add(entity);
            }
            return temp;
        }
        public void UpdateSoftReq(Entity ENTITY)
        {
            foreach (var component in GetComponentsByEntity(ENTITY))
            {
                component.SoftReq(ENTITY);
            }
        }
        public T Get<T>(Entity ENTITY) where T : Component
        {
            return (T)Components[ComponentIndex<T>()][ENTITY.EntityId];
        }
        public List<Entity> GetEntitiesByMask(Mask MASK)
        {
            List<Entity> temp = new List<Entity>();
            foreach (var entity in Entities)
            {
                if (entity.Mask.FullCheck(MASK.Value)) temp.Add(entity);
            }
            return temp;
        }
        private void AddToTagLists(Entity ENTITY)
        {
            for (int i = 0; i < Tag.TotalTags; i++)
            {
                if (ENTITY.Tag.Check(1 << i)) {
                    TagLists[i].Add(ENTITY);
                    AreAnyUnsorted = true;
                    Unsorted[i] = true;
                }
            }
        } 
        public List<Entity> GetEntitiesByTag(string NAME)
        {
            return TagLists[Tag.Get(NAME).ID];
        }   
        internal void SortLists()
        {
            if (AreAnyUnsorted)
            {
                for (int i = 0; i < TagLists.Length; i++)
                {
                    if (Unsorted[i])
                    {
                        TagLists[i].Sort(Entity.CompareDepth);
                        Unsorted[i] = false;
                    }
                }
                AreAnyUnsorted = false;
            }
        }
        public void SetActualDepth(Entity ENTITY)
        {
            const double theta = 0.000001f;

            double add = 0;
            if (TrueDepths.TryGetValue(ENTITY.depth, out add))
                TrueDepths[ENTITY.depth] += theta;
            else
                TrueDepths.Add(ENTITY.depth, theta);

            ENTITY.TrueDepth = ENTITY.depth - add;

            for (int i = 0; i < Tag.TotalTags; i++)
            {
                if (ENTITY.Tag.Check(1 << i))
                {
                    Unsorted[i] = true;
                    AreAnyUnsorted = true;
                }
            }
        }
        public int GenerateId()
        {
            if (FreeIds.Count > 0)
            {
                int id = FreeIds[0];
                FreeIds.RemoveAt(0);
                return id;
            }
            else
            {
                return previousId++;
            }
        }
        public static void LogCompTypes()
        {
            foreach (var keyValue in ComponentTypes)
            {
                Debug.WriteLine(keyValue.Key.ToString() + " : " + keyValue.Value);
            }
        }
    }
}
