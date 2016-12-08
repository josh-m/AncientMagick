using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace AncientMagick.Comps
{
    public class CompProperties_SpellCaster : CompProperties
    {
        public List<string> spellList = new List<string>();

        public CompProperties_SpellCaster()
        {
            compClass = typeof(CompSpellCaster);
        }
    }
}
