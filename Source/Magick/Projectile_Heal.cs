using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;

namespace Magick
{
    class Projectile_Heal : Projectile
    {
        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);

            if (hitThing is Pawn)
            {
                Pawn pawn = (Pawn)hitThing;
                foreach (var hediff in pawn.health.hediffSet.GetHediffs<Hediff_Injury>())
                {
                    if (!hediff.IsOld() &&
                        (hediff.IsNaturallyHealing() || hediff.NotNaturallyHealingBecauseNeedsTending()))
                    {
                        int healValue = 1 + Mathf.RoundToInt(Rand.Value * 4.0f);
                        pawn.health.HealHediff(hediff, healValue);
                    }
                }
            }
        }
    }
}
