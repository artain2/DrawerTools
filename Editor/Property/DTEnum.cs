using System;
using UnityEditor;

namespace DrawerTools
{
    public class DTEnum<T> : DTProperty where T : Enum
    {
        public override event Action OnValueChanged;
        T value;
        public T Value { get => value; set => SetValue(value); }
        public override object UncastedValue { get => Value; set => SetValue((T)value); }

        public DTEnum(string text) : base(text) { }

        public DTEnum(string text, T val) : base(text) => Value = val;

        public void SetValue(T value)
        {
            var prev = this.value;
            this.value = value;
            if (!prev.Equals(value))
            {
                OnValueChanged?.Invoke();
            }
        }

        public DTEnum<T> AddEnumChangeListener(Action<T> callback)
        {
            // TODO cace callbacks to removce them later in Remove***ChangeListener
            AddChangeListener(() => callback(value));
            return this;
        }

        public DTEnum<T> RemoveEnumChangeListener(Action<T> callback)
        {
            throw new NotImplementedException(); // TODO
        }

        protected override void AtDraw()
        {
            Value = (T)EditorGUILayout.EnumPopup(_guiContent, Value, Sizer.Options);
        }
    }

}