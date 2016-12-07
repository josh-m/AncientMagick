using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Verse;
using RimWorld;
using Verse.AI;

namespace AncientMagick
{

    class DamageWorker_Reanimate : DamageWorker
    {
        IntVec3 pos;

        public override float Apply(DamageInfo dinfo, Thing thing)
        {
            Corpse corpse = thing as Corpse;
            if (corpse != null)
            {
                this.pos = new IntVec3(corpse.Position.x,corpse.Position.y,corpse.Position.z);

                Pawn dead_pawn = corpse.innerPawn;
                //corpse.Destroy(DestroyMode.Vanish);
                Reanimate(dead_pawn);
            }

            return base.Apply(dinfo, thing);
        }

        private void Reanimate(Pawn dead_pawn)
        {
            //Pawn to_spawn = PawnGenerator.GeneratePawn(PawnKindDef.Named("Colonist"));
            //TODO: save existing maladies
            //TODO: readd nonfatal maladies
            //GenSpawn.Spawn(to_spawn, this.pos);

            //return to_spawn;

            //dead_pawn.health.Reset(); //stop being dead


            dead_pawn.mindState = new Verse.AI.Pawn_MindState(dead_pawn);
            if (dead_pawn.stances == null)
            {
                dead_pawn.stances = new Pawn_StanceTracker(dead_pawn);
            }
            if (dead_pawn.needs == null)
            {
                dead_pawn.needs = new Pawn_NeedsTracker(dead_pawn);
            }
            if (dead_pawn.ageTracker == null)
                Log.Message("ageTracker null");

            //change private healthState field to not dead
            FieldInfo health_state = typeof(Pawn_HealthTracker).GetField("healthState", BindingFlags.Instance | BindingFlags.NonPublic);
            health_state.SetValue(dead_pawn.health, PawnHealthState.Mobile);

            //Cure damage enough to stay alive
            List<Hediff> hediffs = dead_pawn.health.hediffSet.hediffs;
            
            //cure hediffs with a lethal severity
            for (int i = 0; i < hediffs.Count; i++)
            {
                if (!(hediffs[i] is Hediff_MissingPart) && hediffs[i].Visible)
                {
                    if (hediffs[i].def.lethalSeverity > -1 && (hediffs[i].Severity >= hediffs[i].def.lethalSeverity))
                    {
                        //Log.Message($"{hediffs[i].def.label} lethal severity: { hediffs[i].def.lethalSeverity}");
                        //Log.Message($"{hediffs[i].Part.def.label} {hediffs[i].def.label}: {hediffs[i].DebugString()}");
                        hediffs[i].Severity = hediffs[i].def.lethalSeverity - 0.1f;
                        //Log.Message($"updated: {hediffs[i].Part.def.label} {hediffs[i].def.label}: {hediffs[i].DebugString()}");
                    }
                    if (!(hediffs[i] is Hediff_MissingPart) && hediffs[i].Visible)
                    {
                        if (hediffs[i].Severity > 0f)
                        {
                            //Log.Message($"{hediffs[i].Part.def.label} {hediffs[i].def.label}: {hediffs[i].DebugString()}");
                            hediffs[i].Severity -= 4f;
                            if (hediffs[i].Severity < 0f)
                                hediffs[i].Severity = 0f;
                            //Log.Message($"updated: {hediffs[i].Part.def.label} {hediffs[i].def.label}: {hediffs[i].DebugString()}");
                        }
                    }
                }
            }


            //reset summary health
            //dead_pawn.health.summaryHealth = new SummaryHealthHandler(dead_pawn);




            //PawnComponentsUtility.CreateInitialComponents(dead_pawn);
            GenSpawn.Spawn(dead_pawn, this.pos);
            //BodyPartRecord brain_record = (BodyPartRecord)BodyDefOf.Human.AllParts
            //    .Where(p => p.def.Equals(BodyPartDefOf.Brain));
            //dead_pawn.health.AddHediff(HediffDef.Named("MagickLifeforce"),brain_record );
            resolveNullPostSpawn(dead_pawn);


            dead_pawn.health.AddHediff(
                HediffDef.Named("MagickLifeforce"),
                dead_pawn.health.hediffSet.GetBrain());
           
            
        }

        private void resolveNullPostSpawn(Pawn p)
        {
            if (p.pather == null)
                p.pather = new Pawn_PathFollower(p);
            if (p.records == null)
                p.records = new Pawn_RecordsTracker(p);
            if (p.carrier == null)
                p.carrier = new Pawn_CarryTracker(p);
            if (p.workSettings == null) {
                p.workSettings = new Pawn_WorkSettings(p);
                p.workSettings.EnableAndInitialize();
            }
        }

    }
}
