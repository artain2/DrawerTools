using System;
using UnityEngine;

namespace DrawerTools
{
    public class DTButton : ButtonDrawerBase
    {
        public static Color DefaultColor = new GUIStyle("Button").normal.textColor;
        public override event Action<ButtonDrawerBase> OnClick;

        public DTButton(string lbl, string tooltip = null) : base(lbl, tooltip) { }

        public DTButton(Texture tex, string tooltip = null) : base(tex, tooltip) { }

        public DTButton(string lbl, Action onClick, string tooltip = null) : base(lbl, onClick, tooltip) { }

        public DTButton(Texture tex, Action onClick, string tooltip = null) : base(tex, onClick, tooltip) { }

        public DTButton(FontIconType icon, string tooltip = null) : this(icon, null, true, tooltip) { }

        public DTButton(FontIconType icon, Action onClick, string tooltip = null) : this(icon, onClick, true, tooltip) { }

        public DTButton(FontIconType icon, Action onClick, bool hideBorders, string tooltip = null) : base("", onClick, tooltip) => SetFontIcon(icon, hideBorders);

        protected override void AtDraw()
        {
            DrawDefaultButton();
        }

        protected override void ClickAction()
        {
            OnClick?.Invoke(this);
        }

        protected override void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
            Style.normal.textColor = enabled ? Color.black : Color.gray;
            Style.active.textColor = enabled ? Color.black : Color.gray;
        }
    }
}