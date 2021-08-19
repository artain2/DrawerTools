using UnityEngine;
using UnityEditor;
using System;

namespace DrawerTools
{

    public class DTInt : DTProperty
    {

        public override event Action OnValueChanged;

        private int value;

        public int Value { get => value; set => SetValue(value); }
        public override object UncastedValue { get => Value; set => SetValue((int)value); }
        public bool IsSlider { get; private set; }
        public int MinSliderValue { get; private set; }
        public int MaxSliderValue { get; private set; }

        public void SetValue(int value)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value)
                OnValueChanged?.Invoke();
        }

        public DTInt(string text) : base(text) { }
        public DTInt(string text, int val) : base(text) => Value = val;

        public DTInt AddIntChangeListener(Action<int> callback)
        {
            // TODO cace callbacks to removce them later in RemoveIntChangeListener
            AddChangeListener(() => callback(value));
            return this;
        }

        public DTInt RemoveIntChangeListener(Action<int> callback)
        {
            throw new NotImplementedException(); // TODO
        }

        protected override void AtDraw()
        {
            if (IsSlider)
            {
                Value = EditorGUILayout.IntSlider(Value, MinSliderValue, MaxSliderValue, Sizer.Options);
            }
            else
            {
                Value = EditorGUILayout.IntField(content, Value, Sizer.Options);
            }
        }

        public DTInt SetViewSlider(int min, int max)
        {
            IsSlider = true;
            MinSliderValue = min;
            MaxSliderValue = max;
            value = Mathf.Clamp(value, min, max);
            return this;
        }

        public DTInt SetViewNormal()
        {
            IsSlider = false;
            return this;
        }
    }
}