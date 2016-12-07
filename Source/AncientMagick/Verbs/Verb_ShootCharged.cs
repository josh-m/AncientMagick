using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using RimWorld;
using Verse;

using AncientMagick.Comps;


namespace AncientMagick
{
    public class Verb_ShootCharged : Verb_Shoot
    {
        protected CompChargeUser compChargeInt = null;
        protected CompChargeUser compCharge
        {
            get
            {
                if (compChargeInt == null && ownerEquipment != null)
                {
                    compChargeInt = ownerEquipment.TryGetComp<CompChargeUser>();
                }
                return compChargeInt;
            }
        }


        protected override bool TryCastShot()
        {
            //Reduce charge
            if (compCharge != null)
            {
                if (base.TryCastShot())
                {
                    compCharge.useCharge();
                    if (!compCharge.hasCharge)
                    {
                        SelfConsume();
                    }
                    return true;
                }

                return false;                
            }

            if (base.TryCastShot())
                return true;
            return false;
        }

        public override void Notify_Dropped()
        {
            base.Notify_Dropped();
            if (this.state == VerbState.Bursting && this.burstShotsLeft < this.verbProps.burstShotCount)
            {
                this.SelfConsume();
            }
        }

        private void SelfConsume()
        {
            if (this.ownerEquipment != null && !this.ownerEquipment.Destroyed)
            {
                Pawn wielder = this.CasterPawn;
                this.ownerEquipment.Destroy(DestroyMode.Vanish);
                ThingWithComps uncharged_staff = (ThingWithComps)ThingMaker.MakeThing(ThingDef.Named("Staff_Arcane"));
                wielder.equipment.MakeRoomFor(uncharged_staff);
                wielder.equipment.AddEquipment(uncharged_staff);
            }
        }
    }

}
