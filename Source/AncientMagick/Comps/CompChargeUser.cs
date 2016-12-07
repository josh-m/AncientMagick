using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using AncientMagick.Gizmos;

namespace AncientMagick.Comps
{
    public class CompChargeUser : ThingComp
    {
        private int curChargeCountInt;

        public CompProperties_ChargeUser Props
        {
            get
            {
                return (CompProperties_ChargeUser)props;
            }
        }

        public int curCharge
        {
            get
            {
                return curChargeCountInt;
            }
        }

        public bool hasCharge
        {
            get
            {
                if (curChargeCountInt > 0)
                    return true;
                return false;
            }
        }

        public CompEquippable compEquippable
        {
            get { return parent.GetComp<CompEquippable>(); }
        }
        public Pawn wielder
        {
            get
            {
                if (compEquippable == null || compEquippable.PrimaryVerb == null)
                {
                    return null;
                }
                return compEquippable.PrimaryVerb.CasterPawn;
            }
        }

        //save/load charge state
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.LookValue<int>(ref this.curChargeCountInt, "currentCharge", 5, false);
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            curChargeCountInt = Props.chargeCount;
        }
        
        public override IEnumerable<Command> CompGetGizmosExtra()
        {
            GizmoChargeStatus chargeStatusGizmo = new GizmoChargeStatus { compCharge = this };
            yield return chargeStatusGizmo;
        }

        public void useCharge()
        {
            if (wielder == null)
            {
                Log.Error(parent.ToString() + " tried reducing its ammo count without a wielder");
            }

            this.curChargeCountInt--;
        }

    }
}
