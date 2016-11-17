using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Magick
{
    class Projectile_Frost: Projectile_Explosive
    {
        protected override void Explode()
        {
            base.Explode();
            SnowUtility.AddSnowRadial(
                base.Position, this.def.projectile.explosionRadius+1, 0.52f);
            GenTemperature.PushHeat(base.Position, -120.0f);
        }
    }
}
