using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace AncientMagick
{
    public class CompProperties_ChargeUser : CompProperties
    {
        public int chargeCount = 3;

        public CompProperties_ChargeUser()
        {
            compClass = typeof(CompChargeUser);
        }
    }
}
