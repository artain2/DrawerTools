using UnityEngine;
using UnityEditor;
using System;

namespace DrawerTools
{
    /// <summary>
    /// Кисть для сцены
    /// Когда включена перехватывает контрол мыши и рейкастит в сцену, чтобы найти объект с коллайдером и указанным скриптом
    /// Если находит - оповещает какой кнопкой мыши был нажат объект
    /// </summary>
    /// <typeparam name="T">Искомый скрипт</typeparam>
    public class DTBrush<T> where T : MonoBehaviour
    {
        /// <summary>
        /// При активной кисти мышь с зажатой кнопкой находится над объектом.
        /// Можно подписать в конструкторе
        /// </summary>
        public event Action<T> OnMainMouse, OnSecondMouse;

        private bool active;
        private bool mainMouseButtonPressed;
        private bool secondMouseButtonPressed;
        private bool insideWindow = false;

        /// <summary>
        /// Вкл/Выкл кисть
        /// </summary>
        public bool IsActive { get => active; set => SetActive(value); }

        /// <summary>
        /// Прямо сейчас идут рейкасты
        /// </summary>
        public bool IsPaintingNow => IsActive && (mainMouseButtonPressed || secondMouseButtonPressed);

        public DTBrush() { }

        public DTBrush(Action<T> mainAction) : this()
        {
            if (mainAction != null)
                OnMainMouse += mainAction;
        }

        public DTBrush(Action<T> mainAction, Action<T> secondAction) : this(mainAction)
        {
            if (secondAction != null)
                OnSecondMouse += secondAction;
        }

        public void SetActive(bool active)
        {
            this.active = active;
            insideWindow = false;
            if (active)
                SceneView.duringSceneGui += AtSceneViewBrush;
            else
                SceneView.duringSceneGui -= AtSceneViewBrush;
        }

        private void AtSceneViewBrush(SceneView view)
        {
            Event e = Event.current;

            if (Event.current.type == EventType.MouseLeaveWindow)
            {
                insideWindow = false;
                mainMouseButtonPressed = false;
                secondMouseButtonPressed = false;
                return;
            }
            if (Event.current.type == EventType.MouseEnterWindow)
            {
                insideWindow = true;
                return;
            }

            var controlID = GUIUtility.GetControlID(FocusType.Passive); // Это стандартный контрол мыши от юнити. Он нам не нужен пока кисть активна

            if (!e.isMouse)
                return;

            if (e.type == EventType.MouseDown && insideWindow) // Проглатываем контрол, чтобы небыло стандартной юнити реакции на мышь
            {
                GUIUtility.hotControl = 0;
                e.Use();
                mainMouseButtonPressed = e.button == 0;
                secondMouseButtonPressed = e.button == 1;
            }
            if (e.type == EventType.MouseUp && insideWindow) // Возвращаем контрол
            {
                mainMouseButtonPressed = false;
                secondMouseButtonPressed = false;
                GUIUtility.hotControl = controlID;
                e.Use();
            }
            BrushDraw(view);
        }

        private void BrushDraw(SceneView view)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(worldRay, out var hitInfo, Mathf.Infinity))
            {
                var target = hitInfo.collider.gameObject;
                if (target != null && target.GetComponent<T>() != null)
                {
                    if (mainMouseButtonPressed)
                        OnMainMouse?.Invoke(target.GetComponent<T>());
                    if (secondMouseButtonPressed)
                        OnSecondMouse?.Invoke(target.GetComponent<T>());
                }
            }
        }
    }
}