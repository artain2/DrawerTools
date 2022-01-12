using UnityEngine;
using UnityEditor;
using System;

namespace DrawerTools
{
    public abstract class DTWindow : EditorWindow, IDTPanel
    {
        /// <summary>
        /// При включении выключении игры у эдтитор окон теряется часть ссылок. Костыльный метод: проверки потери данных - наличие/отсутствие пустого класса
        /// </summary>
        private class CheckInit { }

        public event Action OnSizeChange;

        private CheckInit inited;
        private Vector2 tmpSize;

        public virtual string WindowName => "Window";
        public string DisplayedName { get => titleContent.text; set => SetDisplayedName(value); }
        public IDTPanel Parent => null;

        private void Init()
        {
            if (inited != null)
            {
                return;
            }
            AtInit();
            inited = new CheckInit();
        }

        public void SetDisplayedName(string name)
        {
            titleContent = new GUIContent(name);
        }

        public Vector2 GetFixedSize(float? x = null, float? y = null)
        {
            if (!x.HasValue)
            {
                x = position.width;
            }
            if (!y.HasValue)
            {
                y = position.height;
            }
            return new Vector2(x.Value, y.Value);
        }

        private void OnGUI()
        {
            if (inited == null)
            {
                Init();
            }
            CheckSize();
            AtDraw();


        }

        private void CheckSize()
        {
            if (tmpSize == position.size)
            {
                return;
            }
            tmpSize = position.size;
            OnSizeChange?.Invoke();
        }

        protected abstract void AtDraw();

        protected abstract void AtInit();

        public static T Show<T>() where T : DTWindow
        {
            var window = (DTWindow)GetWindow(typeof(T));
            window.DisplayedName = window.WindowName;
            window.Init();
            return window as T;
        }

        //[MenuItem ("Window/My Window")]
        public static T Get<T>() where T : DTWindow => GetWindow(typeof(T)) as T;

    }
}