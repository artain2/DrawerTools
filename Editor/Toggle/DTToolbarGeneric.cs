using UnityEngine;
using System.Linq;
using System;

namespace DrawerTools
{
    public class DTToolbar<T> : DTDrawable where T : Enum
    {
        public event Action<T> OnItemSelected;
        public event Action<DTToolbar<T>> OnClick;

        private string[] buttonNames;
        private T[] usedValues;
        private int activeValue;

        public T[] Values => usedValues;
        public T ActiveValue => usedValues[activeValue];

        public DTToolbar() : this("") { }

        public DTToolbar(string text) : base(text)
        {
            usedValues = (Enum.GetValues(typeof(T)) as T[]);
            buttonNames = GetButtonNames(usedValues);
        }

        public DTToolbar(string text, Action<T> callback) : this(text)
        {
            AddCallback(callback);
        }

        public DTToolbar(string text, Action<DTToolbar<T>> callback) : this(text)
        {
            AddCallback(callback);
        }

        public DTToolbar(Action<T> callback) : this("", callback) { }

        public DTToolbar(Action<DTToolbar<T>> callback) : this("", callback) { }

        public DTToolbar(string text, bool whiteList = true, params T[] values) : base(text)
        {
            if (whiteList)
            {
                usedValues = values;
                buttonNames = GetButtonNames(usedValues);
                return;
            }

            usedValues = (Enum.GetValues(typeof(T)) as T[]).Concat(values).ToArray();
            buttonNames = GetButtonNames(usedValues);
        }

        public DTToolbar(string text, Action<T> callback, bool whiteList = true, params T[] values) : this(text, whiteList, values)
        {
            AddCallback(callback);
        }

        public DTToolbar(string text, Action<DTToolbar<T>> callback, bool whiteList = true, params T[] values) : this(text, whiteList, values)
        {
            AddCallback(callback);
        }

        public DTToolbar(Action<T> callback, bool whiteList = true, params T[] values) : this("", callback, whiteList, values) { }

        public DTToolbar(Action<DTToolbar<T>> callback, bool whiteList = true, params T[] values) : this("", callback, whiteList, values) { }

        public DTToolbar<T> AddCallback(Action<T> callback)
        {
            OnItemSelected += callback;
            return this;
        }

        public DTToolbar<T> AddCallback(Action<DTToolbar<T>> callback)
        {
            OnClick += callback;
            return this;
        }

        public DTToolbar<T> SetActiveValue(int id, bool invokeCallback = true)
        {
            if (id != activeValue && invokeCallback)
            {
                activeValue = id;
                OnItemSelected?.Invoke(usedValues[id]);
            }

            return this;
        }

        public DTToolbar<T> SetActiveValue(T value, bool invokeCallback = true)
        {
            var ind = Array.IndexOf(usedValues, value);
            if (ind != activeValue)
            {
                activeValue = ind;
                if (invokeCallback)
                {
                    OnItemSelected?.Invoke(value);
                }
            }
            return this;
        }

        protected override void AtDraw()
        {
            var newValue = GUILayout.Toolbar(activeValue, buttonNames, Sizer.Options);
            if (activeValue != newValue)
            {
                activeValue = newValue;
                OnItemSelected?.Invoke(usedValues[activeValue]);
            }
        }

        private string[] GetButtonNames(T[] arr) => usedValues.Select(x => x.ToString()).ToArray();
    }
}