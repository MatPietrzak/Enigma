using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Enigma.Logic
{
    public class Rotor
    {
        private int offset;
        private char label;
        private string layout;
        private Rotor previous, next;
        private char cIn = '\0', notchPosition;
        public Rotor(char label, string layout, char notchPosition)
        {
            this.label = label;
            this.layout = layout;
            this.notchPosition = notchPosition;
            offset = label-65;

            this.previous = previous;
            this.next = next;
        }

        public char GetInverseArray(string x)
        {
            int position = layout.IndexOf(x);

            if (offset > position) position = 26 - (offset - position);
            else position = position - offset;

            if (previous != null) position = (position + previous.GetOffset()) % 26;

            return (char)(position + 65);
        }

        public int GetOffset()
        {
            return offset;
        }
        public string GetOffset2()
        {
            if(offset<9)
            return "0"+(offset+1).ToString();
            return (offset+1).ToString();
        }
        public void UpdateOffset(int offset)
        {
            this.offset = (26+offset) % 26;
            label = (char)(this.offset + 65);
        }

        public char GetnotchPosition()
        {
            return notchPosition;
        }
        public void UpdatenotchPosition(char notch)
        {
            notchPosition=notch;
        }
        public void Updatelayout(string layout)
        {
            this.layout = layout;
        }

        public void Move()
        {
            if (next == null) return;

            offset++;

            if (offset == 26) offset = 0;

            if (next != null && (offset + 66) == ((notchPosition - 64) % 26) + 66)
            {
                next.Move();
            }

            label = (char)(offset + 65);
        }

        public void MoveBack()
        {
            if (offset == 0) offset = 26;

            offset--;

            label = (char)(offset + 65);
        }

        public void DataInput(char input)
        {
            cIn = input;
            char put = input;

            put = (char)(((put - 65) + offset) % 26 + 65);

            if (next != null)
            {
                put = layout.Substring((put - 65), 1).ToCharArray()[0];

                if ((((put - 65) + (-offset)) % 26 + 65) >= 65) put = (char)(((put - 65) + (-offset)) % 26 + 65);
                else put = (char)(((put - 65) + (26 + (-offset))) % 26 + 65);

                next.DataInput(put);
            }
        }

        public char DataOut()
        {
            char otu = '\0';

            if (next != null)
            {
                otu = next.DataOut();
                otu = GetInverseArray("" + otu);
            }
            else //REFLECTOR 
            {
                otu = layout.Substring((cIn - 65), 1).ToCharArray()[0];
                otu = (char)(((otu - 65) + previous.offset) % 26 + 65);
            }

            return otu;
        }

        public void SetNextRotor(Rotor next)
        {
            this.next = next;
        }

        public void SetPreviousRotor(Rotor previous)
        {
            this.previous = previous;
        }
    }

}
