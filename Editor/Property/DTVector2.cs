using System;
using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public class DTVector2 : DTProperty
    {
        public override event Action OnValueChanged;

        private Vector2 value;

        public Vector2 Value { get => value; set => SetValue(value); }

        public override object UncastedValue { get => Value; set => SetValue((Vector2)value); }

        public void SetValue(Vector2 value)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value)
            {
                OnValueChanged?.Invoke();
            }
        }

        public DTVector2(string text) : base(text) { }

        public DTVector2(string text, Vector2 val) : base(text) => Value = val;

        protected override void AtDraw()
        {
            Value = EditorGUILayout.Vector2Field(content, Value, Sizer.Options);
        }
    }

}