using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace DrawerTools
{
    class DTSubtypeSelector : DTProperty
    {
        public event Action<Type> OnTypeSelected;
        public override event Action OnValueChanged;

        /// <summary>
        /// It is int ID in list (0 is abstract)
        /// </summary>
        public override object UncastedValue { get => selected; set => selected = (int)value; }

        private Type abstract_type;
        private Type selected_type;
        private List<Type> allowed_types;
        private string[] keys;
        private int selected = 0;

        public DTSubtypeSelector(string name, Type abstract_type, Action<Type> callback = null) : base(name)
        {
            this.abstract_type = abstract_type;
            if (callback != null)
            {
                OnTypeSelected += callback;
            }

            allowed_types = new List<Type>();
            List<string> list = new List<string>();
            list.Add($"{abstract_type.Name} (abstract)");
            var subtypes = DTReflections.GetAllSubtypes(abstract_type).Where(x => !x.IsAbstract);
            foreach (var subtype in subtypes)
            {
                allowed_types.Add(subtype);
                list.Add(subtype.Name);
            }
            keys = list.ToArray();
        }
        protected override void AtDraw()
        {
            selected = EditorGUILayout.Popup(Name, selected, keys);
            if (selected != 0)
            {
                OnTypeSelected?.Invoke(allowed_types[selected - 1]);
                OnValueChanged?.Invoke();
            }
        }
    }
}