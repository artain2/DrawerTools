using System;
using UnityEngine;

namespace DrawerTools
{
    public class DTToggleButton : ButtonDrawerBase, IDTToggle
    {
        public override event Action<ButtonDrawerBase> OnClick;
        public event Action<bool> OnUserPressedChanged;
        public event Action<bool> OnPressedChanged;

        public bool Pressed { get; protected set; } = false;

        public DTToggleButton(string lbl, Action<bool> onChange, string tooltip = null) : base(lbl, tooltip)
        {
            if (onChange != null)
            {
                OnPressedChanged += onChange;
            }
        }

        public DTToggleButton(Texture tex, Action<bool> onChange, string tooltip = null) : base(tex, tooltip)
        {
            if (onChange != null)
            {
                OnPressedChanged += onChange;
            }
        }

        public virtual IDTToggle SetPressed(bool pressed, bool is_user_action)
        {
            if (Pressed == pressed)
            {
                return this;
            }

            Pressed = pressed;
            ValidateStyle();

            OnPressedChanged?.Invoke(pressed);
            if (is_user_action)
            {
                OnUserPressedChanged?.Invoke(pressed);
            }
            return this;
        }

        protected override void AtDraw()
        {
            DrawDefaultButton();
        }

        protected override void ClickAction()
        {
            SetPressed(!Pressed, true);
            OnClick?.Invoke(this);
        }

        protected override void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
            Style.normal.textColor = enabled ? Color.black : Color.gray;
            Style.active.textColor = enabled ? Color.black : Color.gray;
        }

        protected virtual void ValidateStyle()
        {
            Style.normal.background = Pressed ? DefaultStyle.onActive.scaledBackgrounds[0] : DefaultStyle.normal.background;
        }
    }
}