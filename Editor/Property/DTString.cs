using UnityEditor;
using System;

namespace DrawerTools
{
    public class DTString : DTProperty
    {
        public override event Action OnValueChanged;

        private string value;

        public string Value { get => value; set => SetValue(value); }

        public override object UncastedValue { get => Value; set => SetValue((string)value); }

        public void SetValue(string value)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value)
            {
                OnValueChanged?.Invoke();
            }
        }

        public DTString(string text) : base(text) { }

        public DTString(string text, string val) : base(text) => Value = val;

        protected override void AtDraw()
        {
            Value = EditorGUILayout.TextField(content, Value, Sizer.Options);
        }
    }

}