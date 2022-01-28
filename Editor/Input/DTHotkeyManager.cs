using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrawerTools
{
    public class DTHotkeyManager
    {
        private List<Hotkey> list = new List<Hotkey>();

        public void Add(Action callback, KeyCode key, bool ctrl = false, bool shift = false, bool alt = false)
        {
            list.Add(new Hotkey(key, callback, ctrl, shift, alt));
        }

        public void Add(Action callback, KeyCode key, string csa)
        {
            csa = csa.ToLower();
            Add(callback, key, csa.Contains("c"), csa.Contains("s"), csa.Contains("a"));
        }

        public void Check()
        {
            var e = Event.current;
            if (!(e is {type: EventType.KeyUp}))
                return;
            var match = new Hotkey(e.keyCode, null, e.control, e.shift, e.alt);
             list.FirstOrDefault(x=>x.Equals(match))?.Invoke();
        }


        private class Hotkey
        {
            public KeyCode Key;
            public bool Ctrl;
            public bool Shift;
            public bool Alt;
            public Action Callback;

            public Hotkey(KeyCode key, Action callback, bool ctrl, bool shift, bool alt)
            {
                Key = key;
                Ctrl = ctrl;
                Shift = shift;
                Alt = alt;
                Callback = callback;
            }

            public void Invoke()
            {
                Callback?.Invoke();
            }

            protected bool Equals(Hotkey other)
            {
                return Key == other.Key && Ctrl == other.Ctrl && Shift == other.Shift && Alt == other.Alt;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Hotkey) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (int) Key;
                    hashCode = (hashCode * 397) ^ Ctrl.GetHashCode();
                    hashCode = (hashCode * 397) ^ Shift.GetHashCode();
                    hashCode = (hashCode * 397) ^ Alt.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}