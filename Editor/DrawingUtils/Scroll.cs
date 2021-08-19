using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public class Scroll
    {
        private Vector2 position;
        private SizeModule sizer = new SizeModule().ExpandHeight(true).ExpandWidth(true);

        public float VerticalScrollWidth => 10f;
        public float HorizontalScrollHeight => 10f;

        public Scroll SetSizer(SizeModule sizer)
        {
            this.sizer = sizer;
            return this;
        }

        public void Begin()
        {
            position = EditorGUILayout.BeginScrollView(position, sizer.Options);
        }

        public void End()
        {
            EditorGUILayout.EndScrollView();
        }
    }
}