using UnityEditor;
using System;

namespace DrawerTools
{
    public class DTBool : DTProperty
    {
        public override event Action OnValueChanged;

        private bool value;

        public bool Value { get => value; set => SetValue(value); }
        public override object UncastedValue { get => Value; set => SetValue((bool)value); }

        public DTBool(string text) : base(text) { }

        public DTBool(string text, bool val) : base(text) => Value = val;

        public void SetValue(bool value)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value)
                OnValueChanged?.Invoke();
        }

        protected override void AtDraw()
        {
            Value = EditorGUILayout.Toggle(content, Value, Sizer.Options);
        }
    }

}