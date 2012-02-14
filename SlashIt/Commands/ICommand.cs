using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public interface ICommand
    {
        bool execute(LocalKeyInfo keyInfo);
    }
}
