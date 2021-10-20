using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Object = UnityEngine.Object;

namespace DrawerTools
{
    public static class DT
    {
        public const float UNITY_SPACING = 3f;

        public static T LoadResource<T>(string name) where T : Object => 
            (T)EditorGUIUtility.Load(name);

        public static void Label(string label, 
            float? width = null, 
            HAligment hor_alig = HAligment.Left, 
            FontStyle font_style = FontStyle.Normal, 
            Color? color = null)
        {
            GUIStyle style = new GUIStyle("Label");
            List<GUILayoutOption> options = new List<GUILayoutOption>();
            if (width != null)
            {
                options.Add(GUILayout.MaxWidth(width.Value));
            }
            if (hor_alig != HAligment.Left)
            {
                style.alignment = hor_alig == HAligment.Centre ? TextAnchor.MiddleCenter : TextAnchor.MiddleRight;
            }
            style.fontStyle = font_style;
            if (color.HasValue)
            {
                style.GetAllStates().ForEach(x => x.textColor = color.Value);
            }

            EditorGUILayout.LabelField(label, style, options.ToArray());
        }

        public static void Space(float size) =>
            GUILayout.Space(size);

        public static void Texture(Texture2D tex, Vector2 size) => 
            GUILayout.Box(tex, GUILayout.Width(size.x), GUILayout.Height(size.y));
       
        public static void Texture(Texture2D tex) =>
            GUILayout.Box(tex);
      
        public static void ShowAsset(Object obj)
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }
      
        public static void ShowFolder(string path)
        {
            EditorUtility.FocusProjectWindow();
            if (path[path.Length - 1] == '/')
            {
                path = path.Substring(0, path.Length - 1);
            }
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }
      
        public static T GetOrCreateConfig<T>(string path) where T : ScriptableObject, new()
        {
            path = path.Contains(".asset") ? path : path + ".asset";
            var cfg = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
            if (cfg == null)
            {
                cfg = new T();
                AssetDatabase.CreateAsset(cfg, path);
            }
            return cfg;
        }
     
        public static DTFlex Flex { get; private set; } = new DTFlex();
    }
}