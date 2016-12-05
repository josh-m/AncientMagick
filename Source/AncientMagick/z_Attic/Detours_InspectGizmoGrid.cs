using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Verse;
using RimWorld;

namespace AncientMagick.Detours
{
    static class Detours_InspectGizmoGrid
    {
        //Reflect MainTabWindow_Inspect class (it is internal static)
        public static Type _InspectGizmoGrid = Type.GetType("RimWorld.InspectGizmoGrid, Assembly-CSharp, Version=0.15.6089.4186, Culture=neutral, PublicKeyToken=null");

        //Reflect required fields from reflected class
        static List<object> objList =
            (List<object>)_InspectGizmoGrid
                .GetField("objList", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

        static List<Gizmo> gizmoList =
            (List<Gizmo>)_InspectGizmoGrid
                .GetField("gizmoList", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);


        private static void DrawInspectGizmoGridFor(this MainTabWindow_Inspect _this, IEnumerable<object> selectedObjects)
        {

            if (_InspectGizmoGrid == null)
            {
                Log.Error("Reflected InspectGizmoGrid not found.");
                return;
            }
            if (objList == null)
            {
                Log.Error("Reflected objList not found.");
                return;
            }
            if (gizmoList == null)
            {
                Log.Error("Reflected gizmoList not found.");
                return;
            }

            try
            {
                Log.Message("Enter Draw Inspect");
                Log.Message($"{selectedObjects.ToString()}");
                if (selectedObjects == null)
                {
                    Log.Message("selectedObjects is null");
                }
                Log.Message("parameter accessed!");
                objList.Clear();
                Log.Message("objList cleared");
                objList.AddRange(selectedObjects);
                Log.Message("objList added selectedObjects");
                gizmoList.Clear();
                Log.Message("Start selectable");
                for (int i = 0; i < objList.Count; i++)
                {
                    ISelectable selectable = objList[i] as ISelectable;
                    if (selectable != null)
                    {
                        foreach (Gizmo current in selectable.GetGizmos())
                        {
                            //Log.Message(current.ToString());
                            gizmoList.Add(current);
                        }
                    }
                }
                Log.Message("End selectable");
                for (int j = 0; j < objList.Count; j++)
                {
                    Thing t = objList[j] as Thing;
                    if (t != null)
                    {
                        List<Designator> allDesignators = ReverseDesignatorDatabase.AllDesignators;
                        for (int k = 0; k < allDesignators.Count; k++)
                        {
                            Designator des = allDesignators[k];
                            if (des.CanDesignateThing(t).Accepted)
                            {
                                Command_Action command_Action = new Command_Action();
                                command_Action.defaultLabel = des.LabelCapReverseDesignating(t);
                                command_Action.icon = des.IconReverseDesignating(t);
                                command_Action.defaultDesc = des.DescReverseDesignating(t);
                                command_Action.action = delegate
                                {
                                    if (!TutorSystem.AllowAction(des.TutorTagDesignate))
                                    {
                                        return;
                                    }
                                    des.DesignateThing(t);
                                    des.Finalize(true);
                                };
                                command_Action.hotKey = des.hotKey;
                                command_Action.groupKey = des.groupKey;
                                gizmoList.Add(command_Action);
                            }
                        }
                    }
                }
                Gizmo gizmo;
                GizmoGridDrawer.DrawGizmoGrid(gizmoList, MainTabWindow_Inspect.PaneSize.x + 20f, out gizmo);
                //Log.Message("end");
            }
            catch (Exception ex)
            {
                Log.ErrorOnce(ex.ToString(), 3427734);
            }
        }
    }
}
