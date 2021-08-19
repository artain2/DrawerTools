using UnityEditor;

namespace DrawerTools
{
    public class DTFlex : DTDrawable
    {
        public DTFlex(float? width = null, float? height = null)
        {
            if (width.HasValue)
            {
                Width = width.Value;
            }
            if (height.HasValue)
            {
                Width = height.Value;
            }
        }

        protected override void AtDraw()
        {
            EditorGUILayout.LabelField("", Sizer.Options);
        }
    }
}