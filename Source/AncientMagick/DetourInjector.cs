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
            // Detour ScatterAt
            if (!Detours.TryDetourFromTo(typeof(RimWorld.GenStep_ScatterShrines).GetMethod("ScatterAt", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(GenStep_ScatterShrines).GetMethod("ScatterAt", BindingFlags.Instance | BindingFlags.NonPublic)))
                return false;
            return true;
        }
    }
}