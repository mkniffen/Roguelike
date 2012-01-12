using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SlashIt
{
    public class Player : Mobile
    {
        public Player()
        {
            Description = "This guy is a newb!!";
            DisplayCharacter = "@";
            Name = "Player";
            UniqueId = Constants.UniqueIds.Player;
        }

        public void Save(TextWriter saveFileWriter)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Player));

            serializer.Serialize(saveFileWriter, this);
        }

        public Player Load(StreamReader saveFileStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Player));
            return (Player)serializer.Deserialize(saveFileStream);
        }
    }
}
