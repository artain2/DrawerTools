using System;
using UnityEditor;

namespace DrawerTools
{
    public class DTPopup : DTProperty
    {

        public override event Action OnValueChanged;
        public event Action<DTPopup> OnPopupValueChanged;

        private int activeValue;
        private string[] values;

        public override object UncastedValue { get => values[activeValue]; set => SelectValue(value as string); }
        public int ActiveValueID => activeValue;
        public string ActiveValue => values[activeValue];


        public DTPopup(string[] val) : base("") => values = val;
        public DTPopup(string[] val, int activeValue) : this(val) => this.activeValue = activeValue;
        public DTPopup(string[] val, int activeValue, Action changeCallback) : this(val, activeValue) => OnValueChanged += changeCallback;
        public DTPopup(string[] val, int activeValue, Action<DTPopup> changeCallback) : this(val, activeValue) => OnPopupValueChanged += changeCallback;
        public DTPopup(string title, string[] val) : base(title) => values = val;
        public DTPopup(string title, string[] val, int activeValue) : this(title, val) => this.activeValue = activeValue;
        public DTPopup(string title, string[] val, int activeValue, Action changeCallback) : this(title, val, activeValue) => OnValueChanged += changeCallback;
        public DTPopup(string title, string[] val, int activeValue, Action<DTPopup> changeCallback) : this(title, val, activeValue) => OnPopupValueChanged += changeCallback;

        protected override void AtDraw()
        {
            SelectValue(EditorGUILayout.Popup(activeValue, values));
        }

        public DTPopup SelectValue(string val, bool invokeCallback = true)
        {
            var prev = activeValue;
            activeValue = Array.IndexOf(values, val);
            if (prev != activeValue && invokeCallback)
            {
                OnValueChanged?.Invoke();
                OnPopupValueChanged?.Invoke(this);
            }
            return this;
        }

        public DTPopup SelectValue(int index, bool invokeCallback = true)
        {
            var prev = activeValue;
            activeValue = index;
            if (prev != activeValue && invokeCallback)
            {
                OnValueChanged?.Invoke();
                OnPopupValueChanged?.Invoke(this);
            }
            return this;
        }

        public DTPopup AddChangeCallback(Action changeCallback)
        {
            OnValueChanged += changeCallback;
            return this;
        }

        public DTPopup AddChangeCallback(Action<DTPopup> changeCallback)
        {
            OnPopupValueChanged += changeCallback;
            return this;
        }
    }
}