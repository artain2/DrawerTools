using System;
using UnityEditor;

using Object = UnityEngine.Object;

namespace DrawerTools
{
    public class DTObject<T> : DTProperty where T : Object
    {
        public override event Action OnValueChanged;

        private T value;
        private bool allowSceneObjects = true;

        public T Value { get => value; set => SetValue(value); }
        public override object UncastedValue { get => Value; set => SetValue((T)value); }
        public void SetValue(T value)
        {
            var prev_value = this.value;
            this.value = value;
            if (prev_value != value)
                OnValueChanged?.Invoke();
        }

        public DTObject(string text) : base(text) { }

        public DTObject(string text, T obj) : base(text) => Value = obj;

        public DTObject<T> SetSceneObjectsAllown(bool allow) { allowSceneObjects = allow; return this; }

        public DTObject<T> AddObjectChangeListener(Action<T> callback)
        {
            // TODO cace callbacks to removce them later in Remove***ChangeListener
            AddChangeListener(() => callback(value));
            return this;
        }

        public DTObject<T> RemoveObjectChangeListener(Action<T> callback)
        {
            throw new NotImplementedException(); // TODO
        }

        protected override void AtDraw()
        {
            if (string.IsNullOrEmpty(content.text))
            {
                Value = (T)EditorGUILayout.ObjectField(Value, typeof(T), allowSceneObjects, Sizer.Options); // Для спрайтов с контентом юнити рисует дичь
            }
            else
            {
                Value = (T)EditorGUILayout.ObjectField(content, Value, typeof(T), allowSceneObjects, Sizer.Options);
            }
        }

    }

}