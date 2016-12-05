using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace AncientMagick.Gizmos
{
    [StaticConstructorOnStartup]
    public class GizmoCastSpell : Command
    {
        private static bool initialized;
        private static new Texture2D BGTex;

        public override float Width
        {
            get
            {
                return 80;
            }
        }


        public override GizmoResult GizmoOnGUI(Vector2 topLeft)
        {
            if (!initialized)
                InitializeTextures();

            Rect overRect = new Rect(topLeft.x, topLeft.y, Width, Height);
            Widgets.DrawBox(overRect);
            GUI.DrawTexture(overRect, BGTex);

            Rect inRect = overRect.ContractedBy(6);

            Rect textRect = inRect;
            textRect.height = overRect.height / 2;
            Text.Font = GameFont.Tiny;
            Widgets.Label(textRect, "Cast Spell");

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
