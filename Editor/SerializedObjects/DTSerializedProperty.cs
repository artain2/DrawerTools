using System.Collections;
using UnityEngine;
using System;

namespace DrawerTools
{
    public abstract class DTSerializedProperty : DTProperty
    {
        public DTSerializedProperty(string text) : base(text) { }

        protected DTProperty GetProperty(string name, Type type, object value)
        {
            if (type.IsEnum)
            {
                return Activator.CreateInstance((typeof(DTEnum<>)).MakeGenericType(type), name, value) as DTProperty;
            }
            if (type == typeof(int))
            {
                return new DTInt(name, (int)value);
            }
            else if (type == typeof(float))
            {
                return new DTFloat(name, (float)value);
            }
            else if (type == typeof(double))
            {
                return new DTDouble(name, (double)value);
            }
            else if (type == typeof(string))
            {
                return new DTString(name, (string)value);
            }
            else if (type == typeof(bool))
            {
                return new DTBool(name, (bool)value);
            }
            else if (type == typeof(Vector2))
            {
                return new DTVector2(name, (Vector2)value);
            }
            else if (type.GetInterface(nameof(IList)) != null)
            {
                return new DTSerrializedList(name, (IList)value, type);
            }
            else if (type.IsSerializable)
            {
                return new DTSerializableObject(name, value, type);
            }
            return null;
        }
    }
}