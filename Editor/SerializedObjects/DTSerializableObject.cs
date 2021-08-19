using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

namespace DrawerTools
{
    public class DTSerializableObject : DTSerializedProperty
    {
        public override event Action OnValueChanged;

        private List<DTProperty> props;
        private DTExpandToggle opener;
        private Type override_type;
        private DTSubtypeSelector subclass_selector;
        private bool selecting_subclass = false;

        public object Value { get; protected set; }
        public override object UncastedValue { get => Value; set => throw new NotImplementedException(); } // Как бы да, но нет

        public DTSerializableObject(string name, object value, Type object_type) : base(name)
        {
            opener = new DTExpandToggle(OpenClose);

            if (object_type.Attributes.HasFlag(TypeAttributes.Abstract))
            {
                InitAbstract(object_type);
                return;
            }

            if (value == null)
            {
                if (object_type.GetInterface(nameof(IList)) != null)
                {
                    value = Activator.CreateInstance(object_type, 0);
                }
                else
                {
                    value = Activator.CreateInstance(object_type);// TODO проверить без пустого конструктора
                }
            }
            InitNormal(value, object_type);
        }

        protected override void AtDraw()
        {
            if (selecting_subclass)
            {
                DrawAsSelector();
            }
            else
            {
                DrawNormal();
            }
        }

        private void InitAbstract(Type abstract_class)
        {
            subclass_selector = new DTSubtypeSelector(Name, abstract_class, ListenOverrideTypeSelected);
            selecting_subclass = true;
        }

        private void InitNormal(object value, Type object_type)
        {
            this.Value = value;
            props = new List<DTProperty>();
            RegistrateFieldsAsDTProperies(value, object_type);
        }

        /// <summary>
        /// Распознает все серриализуемые поля и создает под них дравабельные DTProperty
        /// </summary>
        private void RegistrateFieldsAsDTProperies(object value, Type object_type)
        {
            var all_fields = value.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var all_props = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var serilizable_fields = all_fields.Where(x => x.IsPublic || x.CustomAttributes.Any(atr => atr.AttributeType == typeof(SerializeReference) || atr.AttributeType == typeof(SerializeField)));
            var serilizable_props = all_props.Where(x => x.CustomAttributes.Any(atr => atr.AttributeType == typeof(SerializeReference) || atr.AttributeType == typeof(SerializeField)));

            foreach (var fieldInfo in serilizable_fields)
            {
                DTProperty prop = GetProperty(fieldInfo.Name, fieldInfo.FieldType, fieldInfo.GetValue(value));
                if (prop == null)
                {
                    continue;
                }
                prop.AddChangeDelegate(value, fieldInfo.Name);
                prop.OnValueChanged += OnValueChanged;
                props.Add(prop);
            }
            foreach (var propInfo in serilizable_props)
            {
                DTProperty prop = GetProperty(propInfo.Name, propInfo.PropertyType, propInfo.GetValue(value));
                if (prop == null)
                {
                    continue;
                }
                prop.AddChangeDelegate(value, propInfo.Name);
                prop.OnValueChanged += OnValueChanged;
                props.Add(prop);
            }
            foreach (var prop in props)
            {
                prop.OnBeforeDraw += PropPrefix;
                prop.OnAfterDraw += PropPostfix;
            }
        }

        private void ListenOverrideTypeSelected(Type override_type)
        {
            this.override_type = override_type;
            selecting_subclass = false;
            Value = Activator.CreateInstance(override_type);// TODO проверить без пустого конструктора
            OnValueChanged?.Invoke();
            InitNormal(Value, override_type);
        }

        private void ListenValueChanged(FieldInfo field, object new_val)
        {
            field.SetValue(Value, new_val);
            OnValueChanged?.Invoke();
        }

        private void DrawAsSelector()
        {
            using (DTScope.Vertical)
            {
                subclass_selector.Draw();
            }
        }

        private void DrawNormal()
        {
            using (DTScope.Vertical)
            {
                using (DTScope.Horizontal)
                {
                    opener.Draw();
                    DT.Label(Name);
                }

                if (opener.Pressed)
                {
                    DrawProps();
                }
            }
        }

        private void PropPrefix()
        {
            DTScope.Begin(Scope.Horizontal);
            DTSeparators.DrawVerticalSeparator(1);
        }

        private void PropPostfix()
        {
            DTScope.End(Scope.Horizontal);
        }

        private void DrawProps()
        {
            using (DTScope.Vertical)
            {
                for (int i = 0; i < props.Count; i++)
                {
                    props[i].Draw();
                }
            }
        }

        private void OpenClose(bool open) { }
    }
}