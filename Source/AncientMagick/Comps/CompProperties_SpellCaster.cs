using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace AncientMagick.Comps
{
    public class CompProperties_SpellCaster : CompProperties
    {
        public string spellName = "Undefined";

        public CompProperties_SpellCaster()
        {
            compClass = typeof(CompSpellCaster);
        }
    }
}
