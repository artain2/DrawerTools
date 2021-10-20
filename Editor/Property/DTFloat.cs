using System;
using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public class DTFloat : DTProperty
    {
        public override event Action OnValueChanged;

        public bool drawAsSlider = false;
        private float value;
        private float min = float.MinValue;
        private float max = float.MaxValue;

        public override object UncastedValue { get => Value; set => SetValue((float)value); }
        public float Value { get => value; set => SetValue(value); }

        public DTFloat(string text) : base(text) { }

        public DTFloat(string text, float val) : base(text) => Value = val;
        public DTFloat(float val) : this("", val) { }

        public DTFloat SetClamped(float min, float max)
        {
            this.min = min;
            this.max = max;
            return this;
        }

        public void SetValue(float value, bool invokeEvent = true)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value && invokeEvent)
            {
                OnValueChanged?.Invoke();
            }
        }
        public DTFloat AddFloatChangeListener(Action<float> callback)
        {
            // TODO cace callbacks to removce them later in RemoveIntChangeListener
            AddChangeListener(() => callback(value));
            return this;
        }

        public DTFloat RemoveIntChangeListener(Action<float> callback)
        {
            throw new NotImplementedException(); // TODO
        }

        protected override void AtDraw()
        {
            if (drawAsSlider)
            {
                Value = EditorGUILayout.Slider(content, Value, min, max, Sizer.Options);
            }
            else
            {
                Value = Mathf.Clamp(EditorGUILayout.FloatField(content, Value, Sizer.Options), min, max);
            }
        }
    }

}