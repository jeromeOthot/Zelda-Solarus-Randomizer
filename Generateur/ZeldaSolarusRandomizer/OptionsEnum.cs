using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaSolarusRandomizer
{
    [Flags]
    public enum OptionsEnum
    {
        None = 0,
        NoBoss = 1,
        NoImageBoss = 2,
        NoHistory = 4
    }
}