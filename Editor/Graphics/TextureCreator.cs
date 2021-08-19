using UnityEngine;

namespace DrawerTools
{
    static class TextureCreator
    {
        const int SPACING = 18;

        public static GUIStyle CreateStyle(Color col)
        {
            Texture2D tex = CreateTexture(col, SPACING);
            Color[] arr = new Color[SPACING * SPACING];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = col;
            }
            tex.SetPixels(arr);
            tex.Apply();
            GUIStyle style = new GUIStyle("Box");
            style.normal.background = tex;
            return style;
        }

        public static Texture2D CreateTexture(Color col, int size = 100)
        {
            Texture2D tex = new Texture2D(size, size);
            Color[] arr = new Color[size * size];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = col;
            }
            tex.SetPixels(arr);
            tex.Apply();
            return tex;
        }

        public static Sprite CreateSprite(Color col, int size = 100)
        {
            Texture2D tex = CreateTexture(col, 100);
            var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), Vector2.one * .5f);
            return sprite;
        }
    }
}