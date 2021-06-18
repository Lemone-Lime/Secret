using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hasel
{
    public class Tag
    {
        internal static int TotalTags = 0;
        internal static Tag[] ByID = new Tag[32];
        private static Dictionary<string, Tag> ByName = new Dictionary<string, Tag>(StringComparer.OrdinalIgnoreCase);

        public int ID;
        public int Value;
        public string Name;

        public Tag(string NAME)
        {
#if DEBUG
            if (TotalTags >= 32)
                throw new Exception("Maximum tag limit of 32 exceeded!");
            if (ByName.ContainsKey(NAME))
                throw new Exception("Two tags defined with the same name: '" + NAME + "'!");
#endif
            ID = TotalTags;
            Value = 1 << TotalTags;
            Name = NAME;

            ByID[ID] = this;
            ByName[Name] = this;

            TotalTags++;
        }

        public static Tag Get(string NAME)
        {
#if DEBUG
            if (!ByName.ContainsKey(NAME)) throw new Exception("No tag with the name '" + NAME + "' has been defined");
#endif
            return ByName[NAME];
        }
        public static void Log()
        {
            foreach (var keyValue in ByName)
            {
                Debug.WriteLine(keyValue.Key + " : " + keyValue.Value.ID);
            }
        }
    }
}
