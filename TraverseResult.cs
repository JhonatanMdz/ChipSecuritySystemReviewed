using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem
{
    public class TraverseResult
    {
        public bool CanUnlock { get; set; }
        public int ChipsUsed { get; set; }
        public string Path { get; set; }
        public string UnUsedChips { get; set; }
        public bool HasUnUsedChips { get; set; }
    }
}
