using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    /// <summary>
    /// Draw label with some text
    /// </summary>
    public class DTLabel : DTDrawable
    {
        public static Color DefaultColor = new GUIStyle("Label").normal.textColor;

        public GUIStyle Style { get; protected set; } = new GUIStyle("Label");

        public Color Color { get => Style.normal.textColor; set => SetColor(value); }

        public DTLabel SetColor(Color col)
        {
            Style.GetAllStates().ForEach(x => x.textColor = col);
            return this;
        }

        public string Text { get => Name; set => Name = value; }

        public DTLabel(string text) : base(text) { }
        public DTLabel(string text, float width) : base(text)
        {
            SetWidth(width);
        }

        protected override void AtDraw()
        {
            EditorGUILayout.LabelField(Name, Style, Sizer.Options);
        }
    }
}