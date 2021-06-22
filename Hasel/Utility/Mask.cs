using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hasel
{
    public class Mask
    {
        protected int value = 0;
        public virtual int Value { get; set; }

        public bool this[int VALUE]
        {
            get { return (Value & (1 << VALUE)) != 0; }
            set
            {
                if (value) Value += 1 << VALUE;
                else Value -= 1 << VALUE;
            }
        }

        public Mask()
        {

        }
        public Mask(IEnumerable<Type> TYPES)
        {
            foreach (var type in TYPES)
            {
                this[Forge.ComponentTypes[type]] = true;
            }
        }
        public bool FullCheck(int VALUE)
        {
            return (Value & VALUE) == VALUE;
        }
        public bool Check(int VALUE)
        {
            return (Value & VALUE) != 0;
        }
        public void Add(int VALUE)
        {
            Value |= VALUE;
        }
        public void Remove(int VALUE)
        {
            Value &= ~VALUE;
        }
        public BitArray ToBinary()
        {
            BitArray tempBits = new BitArray(Forge.ComponentCount, false);
            for (int i = 0; i < Forge.ComponentCount; i++)
            {
                tempBits[i] = (Value & (1 << i)) != 0;
            }
            return tempBits;
        }
        public void Log()
        {
            Debug.WriteLine(Value);
        }
        public void LogBinary()
        {
            BitArray tempBits = ToBinary();
            for (int i = 0; i < tempBits.Count; i++)
            {
                Debug.Write(Convert.ToInt32(tempBits[i]));
            }
            Debug.WriteLine("");
        }
    }
}
