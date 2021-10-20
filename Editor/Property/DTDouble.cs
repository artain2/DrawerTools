using System;
using UnityEditor;

namespace DrawerTools
{
    public class DTDouble : DTProperty
    {
        public override event Action OnValueChanged;

        private double value;

        public override object UncastedValue { get => Value; set => SetValue((float)value); }
        public double Value { get => value; set => SetValue(value); }

        public DTDouble(string text) : base(text) { }

        public DTDouble(string text, double val) : base(text) => Value = val;

        public void SetValue(double value)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value)
                OnValueChanged?.Invoke();
        }

        protected override void AtDraw()
        {
            Value = EditorGUILayout.DoubleField(content, Value, Sizer.Options);
        }
    }

}