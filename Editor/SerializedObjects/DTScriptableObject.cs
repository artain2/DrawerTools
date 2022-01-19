using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.Utilities;
using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public class DTScriptableObject<T> : DTDrawable where T : ScriptableObject, new()
    {
        private SerializedObject serObj;
        private Dictionary<string, SerializedProperty> fieldsDict = new Dictionary<string, SerializedProperty>();

        public T ScriptableObject { get; protected set; }

        public DTScriptableObject()
        {
            ScriptableObject = new T();
            serObj = new SerializedObject(ScriptableObject);
        }

        public DTScriptableObject(T value)
        {
            ScriptableObject = value;
            serObj = new SerializedObject(ScriptableObject);
        }

        public void DrawField(string fieldName, bool andUpdate = true)
        {
            if (andUpdate)
            {
                serObj.Update();
            }

            EditorGUILayout.PropertyField(fieldsDict[fieldName]);
            if (andUpdate)
            {
                serObj.ApplyModifiedProperties();
            }
        }

        public void SaveAsset(string path, bool select = false)
        {
            AssetDatabase.CreateAsset(ScriptableObject, path);
            if (select)
            {
                DT.ShowAsset(ScriptableObject);
            }
        }

        public DTScriptableObject<T> SerializeField(params string[] fieldNames)
        {
            foreach (var fieldName in fieldNames)
            {
                if (fieldsDict.ContainsKey(fieldName))
                    continue;
                var property = serObj.FindProperty(fieldName);
                fieldsDict.Add(fieldName, property);
            }

            return this;
        }

        public void SerializeAll()
        {
            var fields = ScriptableObject.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var toSerialize = fields.
                Where(x => x.IsPublic || x.GetAttribute<SerializeField>() != null).
                Select(x=>x.Name).ToArray();
            SerializeField(toSerialize);
        }

        public bool HasField(string fieldName) => fieldsDict.ContainsKey(fieldName);

        protected override void AtDraw()
        {
            FixMissingTarget();
            serObj.Update();
            foreach (var item in fieldsDict)
            {
                DrawField(item.Key, false);
            }

            serObj.ApplyModifiedProperties();
        }

        private void FixMissingTarget()
        {
            if (serObj != null && serObj.targetObject != null)
            {
                return;
            }

            if (ScriptableObject == null)
            {
                ScriptableObject = Activator.CreateInstance<T>();
            }

            serObj = new SerializedObject(ScriptableObject);
            var keys = fieldsDict.Keys.ToArray();
            fieldsDict.Clear();
            SerializeField(keys);
        }
    }
}