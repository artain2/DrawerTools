using System;
using UnityEditor;

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
        public DTBool(bool val) : this("", val) { }

        public void SetValue(bool value, bool invokeEvent = true)
        {
            var prev = this.value;
            this.value = value;
            if (prev != value && invokeEvent)
                OnValueChanged?.Invoke();
        }

        public DTBool AddBoolChangeCallback(Action<bool> callback)
        {
            // TODO cace callbacks to removce them later in RemoveIntChangeListener
            AddChangeListener(() => callback(value));
            return this;
        }

        public DTBool RemoveBoolChangeCallback(Action<bool> callback)
        {
            throw new NotImplementedException(); // TODO
        }

        protected override void AtDraw()
        {
            Value = EditorGUILayout.Toggle(content, Value, Sizer.Options);
        }
    }

}