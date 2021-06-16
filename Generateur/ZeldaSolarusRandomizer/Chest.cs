using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaSolarusRandomizer
{
    public class Chest
    {
        public int Id { get; set; }
        public ItemType Type { get; set; } = 0;
        public List<List<ItemType>> Dependencies { get; set; }
        public int Zone { get; set; }
        public string Location { get; set; } = string.Empty;
        public string ParentLocation { get; set; } = string.Empty;
        public bool IsInTemple { get; set; } = false;
        public bool IsVanilla { get; set; } = false;

        public Chest()
        {
            Id = 0;
            Type = 0;
            Dependencies = new List<List<ItemType>>();
            Zone = 0;
            Location = "undefinded";
            IsInTemple = false;
            IsVanilla = false;
        }

        public Chest(int id)
        {
            Id = id;
            Type = 0;
            Dependencies = new List<List<ItemType>>();
            Zone = 0;
            Location = "undefinded";
            IsInTemple = false;
            IsVanilla = false;
        }

        public Chest(int id, ItemType type)
        {
            Id = id;
            Type = type;
            Dependencies = new List<List<ItemType>>();
            Zone = 0;
            Location = "undefinded";
            IsInTemple = false;
            IsVanilla = false;
        }

        public bool IsEmpty()
        {
            return Type == 0;
        }
    }
}
