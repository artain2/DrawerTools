using System.Linq;
using System.Reflection;
using System;

namespace DrawerTools
{
    public static class DTReflections
    {
        public struct FieldTarget
        {
            public object target;
            public string field;
            public bool isField;

            public FieldTarget(object target, string field)
            {
                this.target = target;
                this.field = field;
                isField = true;
            }
            public FieldTarget(object target, string field, bool isField)
            {
                this.target = target;
                this.field = field;
                this.isField = isField;
            }

            public void SetValue(object value)
            {
                if (isField)
                {
                    SetFieldValue(target, field, value);
                }
                else
                {
                    SetPropertyValue(target, field, value);
                }
            }
        }

        public static void SetFieldValue(object target, string name, object value)
        {
            var field = target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            field.SetValue(target, value);
        }

        public static void SetPropertyValue(object target, string name, object value)
        {
            var prop = target.GetType().GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            prop.SetValue(target, value);
        }

        public static object GetFieldValue(object target, string name)
        {
            var field = target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            return field.GetValue(target);
        }

        public static T GetFieldValue<T>(object target, string name)
        {
            var field = target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            return (T)field.GetValue(target);
        }

        public static object GetValue(object target, string name)
        {
            var field = target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                return field.GetValue(target);
            }
            var property = target.GetType().GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                return property.GetValue(target);
            }
            throw new Exception($"No property or field {name} in {target}");
        }

        public static void InvokeMethod(object target, string name, params object[] parametres)
        {
            MethodInfo dynMethod = target.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            dynMethod.Invoke(target, parametres);
        }

        public static Type GetUnityType(string name)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == name)
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        public static Type[] GetAllSubtypes(Type parent_type, bool ignoreGeneric = true)
        {
            Func<Type, bool> preficate;
            if (ignoreGeneric)
            {
                preficate = type => type.IsSubclassOf(parent_type) && !type.ContainsGenericParameters;
            }
            else
            {
                preficate = type => type.IsSubclassOf(parent_type);
            }
            return parent_type.GetTypeInfo().Assembly.GetTypes().Where(preficate).ToArray();
        }

        public static Type[] GetAllInterfaceExtenders(Type interface_type)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => interface_type.IsAssignableFrom(p)).ToArray();
        }

    }
}