using System;
using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public class DTColor : DTProperty
    {
        public override event Action OnValueChanged;

        private Color value;

        public Color Value { get => value; set => SetValue(value); }
        public override object UncastedValue { get => Value; set => SetValue((Color)value); }

        public DTColor(string text) : base(text) { }

        public DTColor(string text, Color col) : base(text) => Value = col;

        public void SetValue(Color value)
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
            Value = EditorGUILayout.ColorField(content, Value, Sizer.Options);
        }
    }

}