using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;


namespace Magick
{
    public class Verb_Lightning : Verb_Shoot
    {
        public override void WarmupComplete()
        {
            base.WarmupComplete();

            

            Pawn target;
            Find.MapPawns.AllPawnsSpawned
                .Where(p => p.IsColonistPlayerControlled)
                .TryRandomElement(out target);
            if (target == null) { return; }

            IntVec3 strikePos = new IntVec3(
                target.Position.x,
                0,
                target.Position.z);

            WeatherEventHandler handler = Find.WeatherManager.eventHandler;
            for (int i = 0; i < 5; i++)
            {
                handler.AddEvent(
                    new WeatherEvent_LightningStrike(strikePos));
            }

        }
        
    }
}
