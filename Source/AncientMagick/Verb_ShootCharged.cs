using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;


namespace AncientMagick
{
    public class Verb_ShootCharged : Verb_Shoot
    {



        protected override bool TryCastShot()
        {
            //Reduce ammunition
            if (compAmmo != null)
            {
                if (base.TryCastShot())
                {
                    compAmmo.useCharge();
                    if (!compAmmo.hasCharge)
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

        // Ammo variables
        protected CompChargeUser compAmmoInt = null;
        protected CompChargeUser compAmmo
        {
            get
            {
                if (compAmmoInt == null && ownerEquipment != null)
                {
                    compAmmoInt = ownerEquipment.TryGetComp<CompChargeUser>();
                }
                return compAmmoInt;
            }
        }
    }

}
