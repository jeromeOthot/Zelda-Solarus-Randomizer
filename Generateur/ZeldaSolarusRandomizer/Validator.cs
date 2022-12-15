using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaSolarusRandomizer
{
    public class Validator
    {
        public List<int> ChestPool = new List<int>();
        public const int NB_CHEST = 166;
        Randomizer randomizer { get; set; }

        public Validator(Randomizer randomizer)
        {
            this.randomizer = randomizer;
        }
    }
}
