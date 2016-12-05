using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Verse;
using RimWorld;

namespace AncientMagick
{
    public static class Detours_Pawn_EquipmentTracker
    {

       
        internal static IEnumerable<Gizmo> GetGizmos(this Pawn_EquipmentTracker _this)
        {
            //reflect required methods
            MethodInfo ShouldUseSquadAttackGizmo = typeof(Pawn_EquipmentTracker).GetMethod("ShouldUseSquadAttackGizmo", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo GetSquadAttackGizmo = typeof(Pawn_EquipmentTracker).GetMethod("GetSquadAttackGizmo", BindingFlags.Instance | BindingFlags.NonPublic);

            bool flag = false;

            if ((bool)ShouldUseSquadAttackGizmo.Invoke(_this, null))
            {
                yield return (Gizmo)GetSquadAttackGizmo.Invoke(_this, null);
            }
            int num = 0;
            IEnumerator<ThingWithComps> enumerator = _this.AllEquipment.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ThingWithComps current = enumerator.Current;

                IEnumerator<Gizmo> compGizmosEnumerator = current.GetGizmos().GetEnumerator();
                while (compGizmosEnumerator.MoveNext())
                {
                    yield return compGizmosEnumerator.Current;
                }

                IEnumerator<Command> enumerator2 = current.GetComp<CompEquippable>().GetVerbsCommands().GetEnumerator();
                try
                {
                    uint num2 = 0u;
                    switch (num2)
                    {
                        case 2u:
                            num++;
                            break;
                    }
                    if (enumerator2.MoveNext())
                    {
                        Command current2 = enumerator2.Current;
                        switch (num)
                        {
                            case 0:
                                current2.hotKey = KeyBindingDefOf.Misc1;
                                break;
                            case 1:
                                current2.hotKey = KeyBindingDefOf.Misc2;
                                break;
                            case 2:
                                current2.hotKey = KeyBindingDefOf.Misc3;
                                break;
                        }
                        flag = true;
                        yield return current2;
                        //yield break;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                        if (enumerator2 != null)
                        {
                            enumerator2.Dispose();
                        }
                    }
                }
            }
            yield break;
        }
    }
}
