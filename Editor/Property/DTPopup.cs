using System;
using UnityEditor;

namespace DrawerTools
{
    public class DTPopup : DTProperty
    {

        public override event Action OnValueChanged;
        public event Action<DTPopup> OnPopupValueChanged;

        private int _activeValue;
        private string[] _values;

        public override object UncastedValue { get => _values[_activeValue]; set => SelectValue(value as string); }
        public int ActiveValueID => _activeValue;
        public string ActiveValue => _values[_activeValue];

        public DTPopup() : base("") => _values = new []{""};
        public DTPopup(string[] val) : base("") => _values = val;
        public DTPopup(string[] val, int activeValue) : this(val) => this._activeValue = activeValue;
        public DTPopup(string[] val, int activeValue, Action changeCallback) : this(val, activeValue) => OnValueChanged += changeCallback;
        public DTPopup(string[] val, int activeValue, Action<DTPopup> changeCallback) : this(val, activeValue) => OnPopupValueChanged += changeCallback;
        public DTPopup(string title, string[] val) : base(title) => _values = val;
        public DTPopup(string title, string[] val, int activeValue) : this(title, val) => this._activeValue = activeValue;
        public DTPopup(string title, string[] val, int activeValue, Action changeCallback) : this(title, val, activeValue) => OnValueChanged += changeCallback;
        public DTPopup(string title, string[] val, int activeValue, Action<DTPopup> changeCallback) : this(title, val, activeValue) => OnPopupValueChanged += changeCallback;

        protected override void AtDraw()
        {
            SelectValue(EditorGUILayout.Popup(_activeValue, _values));
        }

        public DTPopup SetValues(string[] values, int selected = 0)
        {
            _values = values;
            _activeValue = selected;
            return this;
        }
        public DTPopup SetValues(string[] values, string selected = null)
        {
            _values = values;
            if (string.IsNullOrEmpty(selected))
            {
                _activeValue = 0;
            }
            else
            {
                SelectValue(selected);
            }

            return this;
        }

        public DTPopup SelectValue(string val, bool invokeCallback = true)
        {
            var prev = _activeValue;
            _activeValue = Array.IndexOf(_values, val);
            if (prev != _activeValue && invokeCallback)
            {
                OnValueChanged?.Invoke();
                OnPopupValueChanged?.Invoke(this);
            }
            return this;
        }

        public DTPopup SelectValue(int index, bool invokeCallback = true)
        {
            var prev = _activeValue;
            _activeValue = index;
            if (prev != _activeValue && invokeCallback)
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