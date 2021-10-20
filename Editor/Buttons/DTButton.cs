using System;
using UnityEngine;

namespace DrawerTools
{
    public class DTButton : ButtonDrawerBase
    {
        public static Color DefaultColor = new GUIStyle("Button").normal.textColor;
        public override event Action<ButtonDrawerBase> OnClick;

        public DTButton(string lbl) : base(lbl) { }

        public DTButton(Texture tex) : base(tex) { }

        public DTButton(string lbl, Action onClick) : base(lbl, onClick) { }

        public DTButton(Texture tex, Action onClick) : base(tex, onClick) { }

        public DTButton(FontIconType icon) : this(icon, null, true) { }

        public DTButton(FontIconType icon, Action onClick) : this(icon, onClick, true) { }

        public DTButton(FontIconType icon, Action onClick, bool hideBorders) : base("", onClick) => SetFontIcon(icon, hideBorders);

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