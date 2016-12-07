using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Verse;
using UnityEngine;

namespace AncientMagick.Detours
{
    static class Detours_GizmoGridDrawer_DrawGizmoGrid
    {
        //reflect required fields

        private static List<List<Gizmo>> gizmoGroups = (List<List<Gizmo>>)typeof(GizmoGridDrawer)
            .GetField("gizmoGroups", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            .GetValue(null);

        private static List<Gizmo> firstGizmos = (List<Gizmo>)typeof(GizmoGridDrawer)
            .GetField("firstGizmos", BindingFlags.Static | BindingFlags.NonPublic)
            .GetValue(null);

        private static float heightDrawn = (float)typeof(GizmoGridDrawer)
            .GetField("heightDrawn", BindingFlags.Static | BindingFlags.NonPublic)
            .GetValue(null);

        private static int heightDrawnFrame = (int)typeof(GizmoGridDrawer)
            .GetField("heightDrawnFrame", BindingFlags.Static | BindingFlags.NonPublic)
            .GetValue(null);

        private static readonly Vector2 GizmoSpacing = (Vector2)typeof(GizmoGridDrawer)
            .GetField("GizmoSpacing", BindingFlags.Static | BindingFlags.NonPublic)
            .GetValue(null);

        internal static void DrawGizmoGrid(IEnumerable<Gizmo> gizmos, float startX, out Gizmo mouseoverGizmo)
        {
            Log.Message("Enter DrawGizmoGrid");
            gizmoGroups.Clear();
            foreach (Gizmo current in gizmos)
            {
                Log.Message(current.ToString());
                bool flag = false;
                for (int i = 0; i < gizmoGroups.Count; i++)
                {
                    if (gizmoGroups[i][0].GroupsWith(current))
                    {
                        flag = true;
                        gizmoGroups[i].Add(current);
                        break;
                    }
                }
                if (!flag)
                {
                    List<Gizmo> list = new List<Gizmo>();
                    list.Add(current);
                    gizmoGroups.Add(list);
                }
            }
            firstGizmos.Clear();
            for (int j = 0; j < gizmoGroups.Count; j++)
            {
                List<Gizmo> source = gizmoGroups[j];
                Gizmo gizmo = source.FirstOrDefault((Gizmo opt) => !opt.disabled);
                if (gizmo == null)
                {
                    gizmo = source.FirstOrDefault<Gizmo>();
                }
                firstGizmos.Add(gizmo);
            }
            GizmoGridDrawer.drawnHotKeys.Clear();
            float num = (float)(Screen.width - 140);
            Text.Font = GameFont.Tiny;
            Vector2 topLeft = new Vector2(startX, (float)(Screen.height - 35) - GizmoSpacing.y - 75f);
            mouseoverGizmo = null;
            Gizmo interactedGiz = null;
            Event ev = null;
            for (int k = 0; k < firstGizmos.Count; k++)
            {
                Gizmo gizmo2 = firstGizmos[k];
                if (gizmo2.Visible)
                {
                    if (topLeft.x + gizmo2.Width + GizmoSpacing.x > num)
                    {
                        topLeft.x = startX;
                        topLeft.y -= 75f + GizmoSpacing.x;
                    }
                    heightDrawnFrame = Time.frameCount;
                    heightDrawn = (float)Screen.height - topLeft.y;
                    GizmoResult gizmoResult = gizmo2.GizmoOnGUI(topLeft);
                    if (gizmoResult.State == GizmoState.Interacted)
                    {
                        ev = gizmoResult.InteractEvent;
                        interactedGiz = gizmo2;
                    }
                    if (gizmoResult.State >= GizmoState.Mouseover)
                    {
                        mouseoverGizmo = gizmo2;
                    }
                    Rect rect = new Rect(topLeft.x, topLeft.y, gizmo2.Width, 75f + GizmoSpacing.y);
                    rect = rect.ContractedBy(-12f);
                    GenUI.AbsorbClicksInRect(rect);
                    topLeft.x += gizmo2.Width + GizmoSpacing.x;
                }
            }
            if (interactedGiz != null)
            {
                List<Gizmo> list2 = gizmoGroups.First((List<Gizmo> group) => group.Contains(interactedGiz));
                for (int l = 0; l < list2.Count; l++)
                {
                    Gizmo gizmo3 = list2[l];
                    if (gizmo3 != interactedGiz && !gizmo3.disabled && interactedGiz.InheritInteractionsFrom(gizmo3))
                    {
                        gizmo3.ProcessInput(ev);
                    }
                }
                interactedGiz.ProcessInput(ev);
                Event.current.Use();
            }
        }
    }
}
