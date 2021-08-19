using System;

namespace DrawerTools
{
    public interface IDTToggle
    {
        event Action<bool> OnPressedChanged;

        event Action<bool> OnUserPressedChanged;

        bool Pressed { get; }

        IDTToggle SetPressed(bool pressed, bool is_user_action);
    }
}