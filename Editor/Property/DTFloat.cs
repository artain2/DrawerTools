using UnityEngine;
using UnityEditor;
using System;

namespace DrawerTools
{
    public class DTFloat : DTProperty
    {
        public override event Action OnValueChanged;

        private float value;
        private float min = float.MinValue;
        private float max = float.MaxValue;

        public override object UncastedValue { get => Value; set => SetValue((float)value); }
        public float Value { get => value; set => SetValue(value); }

        public DTFloat(string text) : base(text) { }

        public DTFloat(string text, float val) : base(text) => Value = val;

        public DTFloat SetClamped(float min, float max)
        {
            this.min = min;
            this.max = max;
            return this;
        }
        
        public void SetValue(float value)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value)
            {
                OnValueChanged?.Invoke();
            }
        }

        protected override void AtDraw()
        {
            Value = Mathf.Clamp(EditorGUILayout.FloatField(content, Value, Sizer.Options), min, max);
        }
    }

}