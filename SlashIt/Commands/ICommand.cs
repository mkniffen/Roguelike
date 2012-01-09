using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public interface ICommand
    {
        void execute(LocalKeyInfo keyInfo);
    }
}
