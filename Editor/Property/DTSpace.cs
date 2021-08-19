using UnityEditor;

namespace DrawerTools
{
    /// <summary>
    /// Draw some empty space (based on element size)
    /// </summary>
    public class DTSpace : DTDrawable
    {
        public DTSpace() : base("") { }

        public DTSpace(string text) : base(text) { }

        protected override void AtDraw()
        {
            EditorGUILayout.LabelField("", Sizer.Options);
        }

        public static DTSpace ToWidth(float width) => new DTSpace().SetWidth(width) as DTSpace;
    }
}