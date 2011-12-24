using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{

    public class Map
    {

        public Map()
        {
            MapOutdated = true;
        }

        public bool MapOutdated { get; set; }

        public int[,] map = new int[10, 10] 
            { { 1,1,1,1,1,1,1,1,1,1 }, 
              { 1,0,0,0,0,0,0,0,1,1 }, 
              { 1,1,1,1,1,1,1,0,1,1 }, 
              { 1,1,1,1,1,1,1,0,1,1 }, 
              { 1,1,0,0,0,0,0,0,1,1 }, 
              { 1,1,1,1,1,0,1,1,1,1 }, 
              { 1,1,1,1,1,0,1,1,1,1 }, 
              { 1,1,1,1,1,0,1,1,1,1 }, 
              { 1,1,0,0,0,0,0,0,0,1 }, 
              { 1,1,1,1,1,1,1,1,1,1 }, 
            };

    }
}
