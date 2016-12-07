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
        public string activeSpell = "None";

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
        }

        public override IEnumerable<Command> CompGetGizmosExtra()
        {
            GizmoCastSpell gizmo = new GizmoCastSpell { compSpells = this };
            yield return gizmo;
        }

    }
}
