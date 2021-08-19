using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System;

namespace DrawerTools
{
    /// <summary>
    /// Size control single property
    /// Adds:
    ///     <see cref="OnValueChanged"/> 
    ///     <see cref="OnUserValueChanged"/> 
    ///     <see cref="UncastedValue"/> - Link to contained value
    /// Realize: 
    ///     <see cref="BeforeDraw"/> - Begins check change value by user
    ///     <see cref="AfterDraw"/> - Ends check change value by user
    /// </summary>
    public abstract class DTProperty : DTDrawable
    {
        public abstract event Action OnValueChanged;
        public event Action OnUserValueChanged;

        List<DTReflections.FieldTarget> changeListeners = new List<DTReflections.FieldTarget>();

        public DTProperty() : this("") { }

        public DTProperty(string text) : base(text)
        {
            OnValueChanged += AtValueChanged;
        }

        public abstract object UncastedValue { get; set; }

        public DTProperty AddChangeListener(Action callback)
        {
            OnValueChanged += callback;
            return this;
        }

        public DTProperty AddUserChangeListener(Action callback)
        {
            OnUserValueChanged += callback;
            return this;
        }

        public DTProperty AddChangeDelegate(object target, string field)
        {
            bool isField = target.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance) != null;
            bool isProp = target.GetType().GetProperty(field, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance) != null;
            if (!isField && !isProp)
            {
                throw new Exception($"{target} not contains field or property with name {field}");
            }
            changeListeners.Add(new DTReflections.FieldTarget(target, field, isField));
            return this;

        }

        public DTProperty RemoveOnChangeDelegate(object target, string field)
        {
            if (!changeListeners.Any(x => x.target == target && x.field == field))
            {
                return this;
            }
            var toRemove = changeListeners.FirstOrDefault(x => x.target == target && x.field == field);
            changeListeners.Remove(toRemove);
            return this;
        }

        protected virtual object GetChangeListenerValue()
        {
            return UncastedValue;
        }

        protected virtual void AtValueChanged()
        {
            changeListeners.ForEach(x => x.SetValue(GetChangeListenerValue()));
        }

        protected override void BeforeDraw()
        {
            base.BeforeDraw();
            EditorGUI.BeginChangeCheck();
        }

        protected override void AfterDraw()
        {
            base.AfterDraw();
            if (EditorGUI.EndChangeCheck())
                OnUserValueChanged?.Invoke();
        }

        public static T DTCreate<T>(object target, string fieldName, params object[] ctorParams) where T : DTProperty
        {
            var prop = Activator.CreateInstance(typeof(T), ctorParams) as T;
            prop.AddChangeDelegate(target, fieldName);
            prop.UncastedValue = DTReflections.GetValue(target, fieldName);
            return prop;
        }
    }
}