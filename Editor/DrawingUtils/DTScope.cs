using UnityEditor;

namespace DrawerTools
{
    public static class DTScope
    {
        public static EditorGUILayout.HorizontalScope Horizontal => new EditorGUILayout.HorizontalScope();

        public static EditorGUILayout.VerticalScope Vertical => new EditorGUILayout.VerticalScope();

        public static EditorGUILayout.VerticalScope Box => new EditorGUILayout.VerticalScope("Box");

        public static EditorGUILayout.HorizontalScope HorizontalBox => new EditorGUILayout.HorizontalScope("Box");

        public static Scroll GetScroll() => new Scroll();

        public static void Begin(Scope scope, bool inBox = false)
        {
            if (scope == Scope.Horizontal)
            {
                EditorGUILayout.BeginHorizontal(inBox ? "Box" : "Label");
            }
            else if (scope == Scope.Vertical)
            {
                EditorGUILayout.BeginVertical(inBox ? "Box" : "Label");
            }
            else if (scope == Scope.HorizontalOffset)
            {
                EditorGUILayout.BeginHorizontal();
                DT.Space(25f);
                EditorGUILayout.BeginVertical(inBox ? "Box" : "Label");
            }
        }

        public static void End(Scope scope)
        {
            if (scope == Scope.Horizontal)
            {
                EditorGUILayout.EndHorizontal();
            }
            else if (scope == Scope.Vertical)
            {
                EditorGUILayout.EndVertical();
            }
            else if (scope == Scope.HorizontalOffset)
            {
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        public static void BeginHorizontalOffset(float offset = 10, bool inBox = true)
        {
            EditorGUILayout.BeginHorizontal();
            DT.Space(offset);
            EditorGUILayout.BeginVertical(inBox ? "Box" : "Label");
        }

        public static void EndHorizontalOffset()
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawHorizontal(params IDTDraw[] toDraw) =>
            Draw(Scope.Horizontal, toDraw);

        public static void DrawHorizontal(string title, params IDTDraw[] toDraw) =>
            Draw(Scope.Horizontal, title, toDraw);

        public static void DrawVertical(params IDTDraw[] toDraw) =>
            Draw(Scope.Vertical, toDraw);

        public static void DrawVertical(string title, params IDTDraw[] toDraw) =>
            Draw(Scope.Vertical, title, toDraw);

        public static void Draw(Scope scope, params IDTDraw[] toDraw)
        {
            Begin(scope);
            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i].Draw();
            }
            End(scope);
        }

        public static void Draw(Scope scope, string title, params IDTDraw[] toDraw)
        {
            Begin(scope);
            DT.Label(title);
            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i].Draw();
            }
            End(scope);
        }

    }
}