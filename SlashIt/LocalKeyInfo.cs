using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class LocalKeyInfo
    {
        public LocalKeyInfo() { ;}

        public LocalKeyInfo(ConsoleKey key, bool shift, bool alt, bool control)
        {
            this.Key = key;

            if (shift)
            {
                this.Modifiers |= ConsoleModifiers.Shift;
            }

            if (alt)
            {
                this.Modifiers |= ConsoleModifiers.Alt;
            }

            if (control)
            {
                this.Modifiers |= ConsoleModifiers.Control;
            }

        }

        public LocalKeyInfo(ConsoleKeyInfo keyInfo)
        {
            this.Key = keyInfo.Key;
            this.Modifiers = keyInfo.Modifiers;
        }

        public ConsoleKey Key { get; set; }
        public ConsoleModifiers Modifiers { get; set; }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            LocalKeyInfo p = obj as LocalKeyInfo;

            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Key == p.Key) && (Modifiers == p.Modifiers);
        }

        public bool Equals(LocalKeyInfo p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Key == p.Key) && (Modifiers == p.Modifiers);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Modifiers.GetHashCode();
        }

    }
}
