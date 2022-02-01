using System;
using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public class DTGameObject : DTProperty
    {
        public override event Action OnValueChanged;

        private GameObject value;

        public GameObject Value { get => value; set => SetValue(value); }
        public override object UncastedValue { get => Value; set => SetValue((GameObject)value); }

        public void SetValue(GameObject value)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value)
            {
                OnValueChanged?.Invoke();
            }
        }

        public DTGameObject(string text) : base(text) { }

        public DTGameObject(string text, GameObject go) : base(text) => Value = go;

        protected override void AtDraw()
        {
            Value = (GameObject)EditorGUILayout.ObjectField(_guiContent, Value, typeof(GameObject), true, Sizer.Options);
        }
    }
}