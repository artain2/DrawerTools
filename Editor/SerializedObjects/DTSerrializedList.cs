using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawerTools
{
    public class DTSerrializedList : DTSerializedProperty, IList
    {
        public override event Action OnValueChanged;

        private List<DTProperty> props;
        private DTExpandToggle opener;
        private Type list_type;
        private Type elem_type;
        private DTButton btn_add;

        public IList Value { get; protected set; }

        public DTSerrializedList(string name, IList value, Type list_type) : base(name)
        {
            this.list_type = list_type;
            if (list_type.IsGenericType)
                elem_type = list_type.GenericTypeArguments[0];
            else
                elem_type = list_type.GetElementType();
            Value = value ?? (IList)Activator.CreateInstance(list_type, 0);

            btn_add = new DTButton("+", AddEmpty) { RectSize = 18 };
            opener = new DTExpandToggle(OpenClose);
            props = new List<DTProperty>();


            for (int i = 0; i < Value.Count; i++)
            {
                AddProperty(GetProperty(GetPropertyName(i), elem_type, Value[i]), i);
            }
        }

        public object this[int index] { get => Value[index]; set => Value[index] = value; }

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public int Count => Value.Count;

        public bool IsSynchronized => (Value as IList).IsSynchronized;

        public object SyncRoot => (Value as IList).SyncRoot;

        public override object UncastedValue { get => Value; set => throw new NotImplementedException(); }

        public int Add(object value)
        {
            int id = Value.Count;
            if (list_type.IsArray)
            {
                Debug.Log("Not implimented");
                return id - 1;
            }
            Value.Add(value);
            AddProperty(GetProperty(GetPropertyName(id), elem_type, Value[id]), id);
            return id;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        protected override void AtDraw()
        {
            using (DTScope.Vertical)
            {
                using (DTScope.Horizontal)
                {
                    opener.Draw();
                    DT.Label(Name, 100);
                    btn_add.Draw();
                }

                if (opener.Pressed)
                    DrawProps();
            }
        }

        private void ListenValueChanged(int id, object new_val)
        {
            Value[id] = new_val;
            OnValueChanged?.Invoke();
        }

        private void AddEmpty()
        {
            var item = Activator.CreateInstance(elem_type);
            Add(item);
        }

        private void AddProperty(DTProperty to_add, int id)
        {
            props.Add(to_add);
            to_add.OnValueChanged += () => ListenValueChanged(id, to_add.UncastedValue);
        }

        private void DrawProps()
        {
            if (Value.Count == 0)
            {
                DT.Label("    Empty");
                return;
            }
            using (DTScope.Vertical)
                for (int i = 0; i < props.Count; i++)
                    props[i].Draw();
        }

        private void OpenClose(bool open) { }

        private string GetPropertyName(int id) => $"  {id + 1}";
    }

}