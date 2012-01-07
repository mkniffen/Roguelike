using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Location
    {
        public Location()
        {
            ;
        }

        public Location (int left, int top)
        {
            this.Left = left;
            this.Top = top;
        }

        public int Top { get; set; }
        public int Left { get; set; }
    }
}
