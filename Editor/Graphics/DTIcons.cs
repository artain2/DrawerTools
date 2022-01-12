using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public static class DTIcons
    {
        private static readonly string[] FontIconKeys = new string[]
        {
            "!", "\"", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/", "0", "1", "2",
            "3", "4", "5", "6", "7", "8", "9", ":", ";", "<", "=", ">", "?", "@", "A", "D", "S", "W", "[", "\\",
            "]", "^", "_", "a", "b", "c", "d", "g", "i", "o", "p", "q", "s", "v", "w", "x", "z", "{", "|", "}"
        };

        private static GUIStyle _red = null;
        private static GUIStyle _green = null;
        private static GUIStyle _yellow = null;

        public static Texture2D GetIcon(IconType type) =>
            (Texture2D) EditorGUIUtility.Load(TypeToPath(type));

        public static Texture2D Red =>
            CachedGet(ref _red, Color.red).normal.background;

        public static Texture2D Green =>
            CachedGet(ref _green, Color.green).normal.background;

        public static Texture2D Yellow =>
            CachedGet(ref _yellow, Color.yellow).normal.background;

        public static GUIStyle FontIconLabelStyle
        {
            get
            {
                DTAssets.TryFindAsset<Font>(
                    "GUIItemsFont",
                    "ttf",
                    out var guiFont,
                    "DrawerTools");
                return new GUIStyle("Label") {font = guiFont, fontSize = 20};
            }
        }

        public static GUIStyle FontIconButtonStyle
        {
            get
            {
                DTAssets.TryFindAsset<Font>(
                    "GUIItemsFont",
                    "ttf",
                    out var guiFont,
                    "DrawerTools");
                return new GUIStyle("Button") {font = DT.LoadResource<Font>("GUIItemsFont.ttf"), fontSize = 20};
            }
        }


        public static void DrawSolidColorIcon(Color color, Vector2 size) =>
            GUILayout.Label("", TextureCreator.CreateStyle(color), GUILayout.Width(size.x), GUILayout.Width(size.y));

        public static void DrawSolidColorIcon(Color color) =>
            DrawSolidColorIcon(color, new Vector2(18, 18));

        public static void DrawRedIcon() =>
            DrawPreset(CachedGet(ref _red, Color.red));

        public static void DrawGreenIcon() =>
            DrawPreset(CachedGet(ref _green, Color.green));

        public static void DrawYellowIcon() =>
            DrawPreset(CachedGet(ref _yellow, Color.yellow));

        public static void DrawFontIcon(FontIconType type)
        {
            EditorGUILayout.LabelField(GetFontIcon(type), FontIconLabelStyle, GUILayout.Width(20f));
        }

        public static string GetFontIcon(FontIconType type) => FontIconKeys[(int) type];

        private static void DrawPreset(GUIStyle style) =>
            GUILayout.Label("", style, GUILayout.Width(18), GUILayout.Width(18));

        private static string TypeToPath(IconType type) =>
            string.Format("EditorIcons/shape{0:000}_style01_color12.png", (int) type);

        private static GUIStyle CachedGet(ref GUIStyle cache, Color to_create)
        {
            if (cache == null)
            {
                cache = TextureCreator.CreateStyle(to_create);
            }

            return cache;
        }
    }
}