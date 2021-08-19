using System;

namespace DrawerTools
{
    public class DTExpandToggle : DTToggleButton
    {
        private FontIconType ico_closed;
        private FontIconType ico_opened;

        public DTExpandToggle() : this(FontIconType.Right, FontIconType.Down, null) { }

        public DTExpandToggle(Action<bool> callback) : this(FontIconType.Right, FontIconType.Down, callback) { }

        public DTExpandToggle(FontIconType closed, FontIconType opened, Action<bool> callback) : base("", callback)
        {
            ico_closed = closed;
            ico_opened = opened;
            Style = DTIcons.FontIconLabelStyle;
            Name = DTIcons.GetFontIcon(closed);
            RectSize = 20;
            SetPressed(false, false);
        }

        public bool DrawAndCheckPress(string text = "")
        {
            DTScope.Begin(Scope.Horizontal);
            this.Draw();
            DT.Label(text);
            DTScope.End(Scope.Horizontal);
            return Pressed;
        }

        public bool DrawAndCheckPress(params IDTDraw[] atRight)
        {
            using (DTScope.Horizontal)
            {
                Draw();
                DTScope.DrawHorizontal(atRight);
            }
            return Pressed;
        }

        protected override void ValidateStyle()
        {
            Name = Pressed ? Name = DTIcons.GetFontIcon(ico_opened) : Name = DTIcons.GetFontIcon(ico_closed);
        }
    }
}