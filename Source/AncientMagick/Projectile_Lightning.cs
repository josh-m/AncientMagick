using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.Sound;

namespace AncientMagick
{
    class Projectile_Lightning : Projectile_Explosive
    {
        protected override void Explode()
        {
            base.Explode();

            if (canStrike(base.Position)){
                WeatherEventHandler handler = Find.WeatherManager.eventHandler;
                for (int i = 0; i < 4; i++)
                {
                    handler.AddEvent(
                        new WeatherEvent_LightningStrike(base.Position));
                }
            }
            else
            {//Play the sound, but don't actually strike
                SoundInfo info = SoundInfo.InWorld(base.Position, MaintenanceType.None);
                SoundDefOf.Thunder_OnMap.PlayOneShot(info);
            }
        }

        private bool canStrike(IntVec3 pos)
        {
            return pos.InBounds() && !pos.Roofed();
        }
    }
}
