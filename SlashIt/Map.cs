using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SlashIt
{

    public class Map : IXmlSerializable
    {
        public Map()
        {
            MapOutdated = true;
            //MapFile = @".\game.sav";
        }

        //public string MapFile { get; set; }

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

        private int[,] map = new int[10, 10];
            //{ { 1,1,1,1,1,1,1,1,1,1 }, 
            //  { 1,0,0,0,0,0,0,0,1,1 }, 
            //  { 1,1,1,1,1,1,1,2,1,1 }, 
            //  { 1,1,1,1,1,1,1,0,1,1 }, 
            //  { 1,1,0,0,0,0,0,0,1,1 }, 
            //  { 1,1,1,1,1,0,1,1,1,1 }, 
            //  { 1,1,1,1,1,0,1,1,1,1 }, 
            //  { 1,1,1,1,1,2,1,1,1,1 }, 
            //  { 1,1,0,0,0,0,0,0,0,1 }, 
            //  { 1,1,1,1,1,1,1,1,1,1 }, 
            //};



        public Map Load(StreamReader saveFileStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            return (Map)serializer.Deserialize(saveFileStream);
        }


        public void Save(TextWriter saveFileWriter)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));

            serializer.Serialize(saveFileWriter, this);
        }


        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }


        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();


            
            //TODO -- Will need to add reading and setting of the Height, Width attributes


            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();

            if (!isEmptyElement)
            {
                var definitionList = reader.ReadElementString("Definition").Split(',');
                int listPosition = 0;

                for (int top = 0; top <= map.GetUpperBound(0); top++)
                {
                    for (int left = 0; left <= map.GetUpperBound(1); left++)
                    {
                        map[top, left] = int.Parse(definitionList[listPosition]);
                        listPosition++;
                    }
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Height", (this.GetUpperBound(0)+1).ToString());
            writer.WriteAttributeString("Width", (this.GetUpperBound(1)+1).ToString());
            writer.WriteElementString("Definition", this.MapToString());
        }

        private string MapToString()
        {
            StringBuilder mapString = new StringBuilder();

            for (int top = 0; top <= this.GetUpperBound(0); top++)
            {
                for (int left = 0; left <= this.GetUpperBound(1); left++)
                {
                    mapString.Append(map[top, left]);
                    mapString.Append(",");
                }
            }

            return mapString.ToString();
        }
    }
}
