using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SlashIt
{

    public class Map
    {

        public Map()
        {
            MapOutdated = true;
        }

        //TODO not sure about this syntax.  Gonna give it a try for now...
        public int this[int top, int left]
        {
            get { return this.map[top,left]; }
            set { this.map[top, left] = value; }
        }

        public bool MapOutdated { get; set; }

        public int GetUpperBound(int dimension)
        {
            return map.GetUpperBound(dimension);
        }

        private int[,] map = new int[10, 10] 
            { { 1,1,1,1,1,1,1,1,1,1 }, 
              { 1,0,0,0,0,0,0,0,1,1 }, 
              { 1,1,1,1,1,1,1,2,1,1 }, 
              { 1,1,1,1,1,1,1,0,1,1 }, 
              { 1,1,0,0,0,0,0,0,1,1 }, 
              { 1,1,1,1,1,0,1,1,1,1 }, 
              { 1,1,1,1,1,0,1,1,1,1 }, 
              { 1,1,1,1,1,2,1,1,1,1 }, 
              { 1,1,0,0,0,0,0,0,0,1 }, 
              { 1,1,1,1,1,1,1,1,1,1 }, 
            };


        public static void Load()
        {
            throw new NotImplementedException();
        }


    }
}
