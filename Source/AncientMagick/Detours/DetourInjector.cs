using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using RimWorld;
using Verse;

namespace AncientMagick.Detours
{
    [StaticConstructorOnStartup]
    internal static class DetourInjector
    {
        static DetourInjector()
        {
            LongEventHandler.QueueLongEvent(Inject, "LibraryStartup", false, null);
        }

        public static void Inject()
        {
            if (InjectDetours()) Log.Message("Detour Core injected.");
            else Log.Error("Detour Core failed to get injected.");
        }

        public static bool InjectDetours()
        {
            if (!Detours.TryDetourFromTo(typeof(RimWorld.GenStep_ScatterShrines).GetMethod("ScatterAt", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(GenStep_ScatterShrines).GetMethod("ScatterAt", BindingFlags.Instance | BindingFlags.NonPublic)))
                return false;
            if (!Detours.TryDetourFromTo(typeof(Verse.Pawn_EquipmentTracker).GetMethod("GetGizmos", BindingFlags.Instance | BindingFlags.Public),
                typeof(Detours_Pawn_EquipmentTracker).GetMethod("GetGizmos", BindingFlags.Static | BindingFlags.NonPublic)))
                return false;
            /*
             * R&D into finding a way to load multiple gizmos on a weapon (for multiple spells tied to a single staff)
             * Solved: Only one gizmo sharing the same label will be drawn, my gizmos were using the same default label
            if (!Detours.TryDetourFromTo(Detours_InspectGizmoGrid._InspectGizmoGrid.GetMethod("DrawInspectGizmoGridFor", BindingFlags.Static | BindingFlags.Public),
                typeof(Detours_InspectGizmoGrid).GetMethod("DrawInspectGizmoGridFor", BindingFlags.Static | BindingFlags.NonPublic)))
                return false;
            if (!Detours.TryDetourFromTo(typeof(GizmoGridDrawer).GetMethod("DrawGizmoGrid", BindingFlags.Static | BindingFlags.Public),
                typeof(Detours_GizmoGridDrawer_DrawGizmoGrid).GetMethod("DrawGizmoGrid", BindingFlags.Static | BindingFlags.NonPublic)))
                return false;
            */

            return true;
        }
    }
}