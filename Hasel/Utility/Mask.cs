using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hasel
{
    public class Mask
    {
        /* Old Method
        public BitArray Bits;
        public const int BitCount = 32;

        public bool this[int index]
        {
            get { if (index < 0 || index > BitCount) throw new IndexOutOfRangeException(); return Bits[index]; }
            set { if (index < 0 || index > BitCount) throw new IndexOutOfRangeException(); Bits[index] = value; }
        }

        public Mask()
        {
            Bits = new BitArray(BitCount, false);
            
        }
        public void Log()
        {
            string temp = "";
            foreach (var bit in Bits)
            {
                temp = temp+Convert.ToInt32(bit);
            }
            Debug.WriteLine(temp);
        }
        public bool Check(Mask MASK) {
            for(var i=0; i<BitCount; i++)
            {
                if (this[i] & MASK[i]) continue;
                return false;
            }
            return true;
        }
        public static implicit operator int(Mask MASK)
        {
            int value = 0;
            for (int i = 0; i < BitCount; i++)
            {
                value += Convert.ToInt32(MASK.Bits[i]) << i;
            }
            return value;
        }
        */
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
