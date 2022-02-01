using System;
using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public class DTVector3 : DTProperty
    {
        public override event Action OnValueChanged;

        private Vector3 value;

        public Vector3 Value { get => value; set => SetValue(value); }

        public override object UncastedValue { get => Value; set => SetValue((Vector3)value); }

        public void SetValue(Vector3 value)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value)
            {
                OnValueChanged?.Invoke();
            }
        }

        public DTVector3(string text) : base(text) { }

        public DTVector3(string text, Vector3 val) : base(text) => Value = val;

        protected override void AtDraw()
        {
            Value = EditorGUILayout.Vector3Field(_guiContent, Value, Sizer.Options);
        }
    }

}