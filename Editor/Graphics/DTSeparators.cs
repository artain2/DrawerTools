using UnityEngine;

namespace DrawerTools
{
    public static class DTSeparators
    {
        private static GUIStyle _vert = null;

        public static GUIStyle Vertical => CachedGet(ref _vert, Color.gray);

        public static void DrawVerticalSeparator(int props_count) =>
            GUILayout.Label("", 
                TextureCreator.CreateStyle(Color.gray), 
                GUILayout.Width(3), 
                GUILayout.Height(props_count * 18)); // TODO заменить на итеративную отрисовку кэшированных текстур
        static GUIStyle CachedGet(ref GUIStyle cache, Color to_create)
        {
            if (cache == null)
            {
                cache = TextureCreator.CreateStyle(to_create);
            }
            return cache;
        }
    }

}