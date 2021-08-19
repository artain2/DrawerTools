using UnityEngine;
using System;

namespace DrawerTools
{
    public class DTToolbar : DTDrawable
    {
        public event Action<DTToolbar> OnClick;

        private string[] buttonNames;
        private int activeValue;

        public int Value => activeValue;
        public string ValueText => buttonNames[activeValue];

        public DTToolbar(string text, params string[] values) : base(text)
        {
            buttonNames = values;
        }

        public DTToolbar(string text, Action<DTToolbar> callback, params string[] values) : base(text)
        {
            buttonNames = values;
            AddCallback(callback);
        }

        public DTToolbar(params string[] values) : this("", values) { }

        public DTToolbar(Action<DTToolbar> callback, params string[] values) : this("", callback, values) { }

        public DTToolbar AddCallback(Action<DTToolbar> callback)
        {
            OnClick += callback;
            return this;
        }

        public DTToolbar SetActiveValue(int id, bool invokeCallback = true)
        {
            if (id != activeValue && invokeCallback)
            {
                activeValue = id;
                OnClick?.Invoke(this);
            }

            return this;
        }

        public DTToolbar SetActiveValue(string value, bool invokeCallback = true)
        {
            var ind = Array.IndexOf(buttonNames, value);
            if (ind != activeValue && invokeCallback)
            {
                activeValue = ind;
                OnClick?.Invoke(this);
            }

            return this;
        }

        protected override void AtDraw()
        {
            var newValue = GUILayout.Toolbar(activeValue, buttonNames, Sizer.Options);
            if (activeValue != newValue)
            {
                activeValue = newValue;
                OnClick?.Invoke(this);
            }
        }
    }
}