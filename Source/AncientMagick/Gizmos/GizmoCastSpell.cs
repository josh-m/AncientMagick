using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

using AncientMagick.Comps;

namespace AncientMagick.Gizmos
{
    [StaticConstructorOnStartup]
    public class GizmoCastSpell : Command
    {
        public CompSpellCaster compSpells;

        private static bool initialized;
        private static new Texture2D BGTex;

        public Action nextSpellAction;

        public GizmoCastSpell()
        {
            base.defaultDesc = "Gizmo to cast a spell";
            base.defaultLabel = "cast spell gizmo";
        }

        public override float Width
        {
            get
            {
                return 80;
            }
        }

        private void NextSpell()
        {
            compSpells.activateNextSpell();
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            this.NextSpell();
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft)
        {
            if (!initialized)
                InitializeTextures();

            Rect overRect = new Rect(topLeft.x, topLeft.y, Width, Height);
            bool mouseFlag = false;
            if (Mouse.IsOver(overRect))
            {
                GUI.color = GenUI.MouseoverColor;
                mouseFlag = true;
            }
            Widgets.DrawBox(overRect);
            GUI.DrawTexture(overRect, BGTex);
            GUI.color = Color.white;

            bool clickFlag = false;
            if (Widgets.ButtonInvisible(overRect))
                clickFlag = true;
            Rect inRect = overRect.ContractedBy(6);
            Rect textRect = inRect;
            textRect.height = overRect.height;
            Text.Font = GameFont.Tiny;
            Widgets.Label(textRect, $"{compSpells.activeSpell}");

            if (clickFlag)
                return new GizmoResult(GizmoState.Interacted, Event.current);
            if (mouseFlag)
                return new GizmoResult(GizmoState.Mouseover);

            else
                return new GizmoResult(GizmoState.Clear);
        }

        private void InitializeTextures()
        {
            if (BGTex == null)
                BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);
            initialized = true;
        }

    }
}
