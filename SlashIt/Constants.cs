using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Constants
    {
        public class UniqueIds
        {
            public const int Floor = 0;
            public const int Wall = 1;
            public const int Door = 2;
            public const int OpenDoor = 3;
            public const int NonPlayerCharacter = 111;  //TODO -- NonPlayerCharacter will likely become a base classe...

            public const int Player = 1000;
        }
    }
}
