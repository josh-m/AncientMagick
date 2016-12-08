using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

using AncientMagick.Gizmos;

namespace AncientMagick.Comps
{
    public class CompSpellCaster : ThingComp
    {
        private int spellCount = 0;

        public int activeSpellIndex = 0;
        public string activeSpell = "None";

        public void activateNextSpell()
        {
            activeSpellIndex++;
            if (activeSpellIndex >= spellCount)
                activeSpellIndex = 0;
            activeSpell = Props.spellList[activeSpellIndex];
        }

        public CompProperties_SpellCaster Props
        {
            get
            {
                return (CompProperties_SpellCaster)props;
            }
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            activeSpellIndex = 0;
            activeSpell = Props.spellList[activeSpellIndex];
            spellCount = Props.spellList.Count;
        }

        public override IEnumerable<Command> CompGetGizmosExtra()
        {
            GizmoCastSpell gizmo = new GizmoCastSpell { compSpells = this };
            yield return gizmo;
        }

    }
}
