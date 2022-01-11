using UnityEngine;

namespace DrawerTools
{
    public static class DTKeys
    {
        public static bool IsControl
        {
            get
            {
                {
                    Event e = Event.current;
                    bool pressed = e != null && e.control;
                    return pressed;
                }
            }
        }
        public static bool IsAlt
        {
            get
            {
                {
                    Event e = Event.current;
                    bool pressed = e != null && e.alt;
                    return pressed;
                }
            }
        }
        public static bool IsShift
        {
            get
            {
                {
                    Event e = Event.current;
                    bool pressed = e != null && e.shift;
                    return pressed;
                }
            }
        }

        public static bool IsKeyPressed(KeyCode key)
        {
            Event e = Event.current;
            bool pressed = e != null && e.keyCode == key;
            return pressed;
        }

        public static bool IsKeyReleased(KeyCode key)
        {
            Event e = Event.current;
            bool pressed = e != null && e.keyCode == key && e.type == EventType.KeyUp;
            return pressed;
        }
    }
}