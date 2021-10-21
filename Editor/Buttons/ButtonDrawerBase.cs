using System;
using UnityEngine;

namespace DrawerTools
{
    public abstract class ButtonDrawerBase : DTDrawable
    {
        public abstract event Action<ButtonDrawerBase> OnClick;

        protected bool highlighted = false;
        protected bool enabled = true;

        protected Action MainClickAction { get; set; }
        protected virtual GUIStyle Style { get; set; } = new GUIStyle("Button");
        protected GUIStyle DefaultStyle { get; } = new GUIStyle("Button");
        public bool Enabled { get => enabled; set => SetEnabled(value); }
        public bool Highlighted { get => highlighted; set => SetHighlighted(value); }

        public ButtonDrawerBase(string lbl, string tooltip = null) : base(lbl, tooltip) { }
        public ButtonDrawerBase(Texture tex, string tooltip = null) : base(tex, tooltip) { }
        public ButtonDrawerBase(Texture tex, Action onClick, string tooltip = null) : base(tex, tooltip)
        {
            MainClickAction = onClick;
            OnClick += (val) => onClick?.Invoke();
        }

        public ButtonDrawerBase(string text, Action onClick, string tooltip = null) : base(text, tooltip)
        {
            MainClickAction = onClick;
            if (onClick != null)
            {
                OnClick += (val) => onClick?.Invoke();
            }
        }

        public virtual ButtonDrawerBase SetTextColor(Color col)
        {
            Style.GetAllStates().ForEach(x => x.textColor = col);
            DefaultStyle.GetAllStates().ForEach(x => x.textColor = col);
            return this;
        }

        public ButtonDrawerBase SetFontIcon(FontIconType type, bool hideBorders = false)
        {
            Style = hideBorders ? DTIcons.FontIconLabelStyle : DTIcons.FontIconButtonStyle;
            Name = DTIcons.GetFontIcon(type);
            return this;
        }

        protected abstract void ClickAction();

        protected void DrawDefaultButton()
        {
            if (GUILayout.Button(content, Style, Sizer.Options) && Enabled)
            {
                ClickAction();
            }
        }

        protected abstract void SetEnabled(bool enabled);

        protected virtual void SetHighlighted(bool value)
        {
            this.highlighted = value;
            Style.fontStyle = value ? FontStyle.BoldAndItalic : FontStyle.Normal;
        }
    }
}