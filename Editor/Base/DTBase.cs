using UnityEngine;
using System.Linq;
using System.Reflection;
using System;
using UnityEditor;

namespace DrawerTools
{
    public abstract class DTBase : IDTDraw
    {
        public event Action OnBeforeDraw, OnAfterDraw;
        public event Action<bool> OnActiveChanged;

        private bool active = true;

        public bool Active { get => active; set => SetActive(value); }
        public virtual EnableInType EnableIn { get; set; }
        public virtual bool Disabled { get; set; }
        public void Draw()
        {
            if (!Active)
            {
                return;
            }
            if (EnableIn == EnableInType.EditorOnly && Application.isPlaying || EnableIn == EnableInType.GameOnly && !Application.isPlaying)
            { 
                return; 
            }

            OnBeforeDraw?.Invoke();
            BeforeDraw();
            EditorGUI.BeginDisabledGroup(Disabled);
            AtDraw();
            EditorGUI.EndDisabledGroup();
            AfterDraw();
            OnAfterDraw?.Invoke();
        }

        public DTBase SetActive(bool value)
        {
            bool prev = active;
            active = value;
            if (prev != active)
            {
                ActiveChanged(active);
                OnActiveChanged?.Invoke(active);
            }
            return this;
        }

        public DTBase SetEnableIn(EnableInType value)
        {
            EnableIn = value;
            return this;
        }
        protected void DrawAll()
        {
            var props = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.FieldType.IsSubclassOf(typeof(DTBase)) || x.FieldType == (typeof(DTBase))).ToArray();
            foreach (var prop in props)
            {
                (prop.GetValue(this) as DTBase).Draw();
            }
        }

        protected virtual void BeforeDraw() { }
        protected abstract void AtDraw();
        protected virtual void AfterDraw() { }
        protected virtual void ActiveChanged(bool active) { }
        public virtual void Destroy() { }

    }
}
