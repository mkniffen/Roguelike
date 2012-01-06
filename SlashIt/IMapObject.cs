using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public interface IMapObject
    {
        string Name { get; set; }
        string Description { get; set; }
        string DisplayCharacter { get; set; }
        int UniqueId { get; set; }
    }
}
