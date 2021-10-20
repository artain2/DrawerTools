using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrawerTools
{
    public class DTTogglePanel : DTBase
    {

        public event Action<int> OnSelectionChange;

        public DTToggleButtonGroup Group { get; protected set; }
        public float Height { get; protected set; } = 18;
        public List<DTToggleButton> Buttons { get; protected set; }
        public bool AllowNotSelected { get => Group.AllowNotSelected; set => Group.AllowNotSelected = value; }


        public DTTogglePanel(params string[] names) => Prepare(18, names.Select(x => new GUIContent(x)).ToArray(), null);

        public DTTogglePanel(string[] names, int selected_panel) => Prepare(18, names.Select(x => new GUIContent(x)).ToArray(), null, selected_panel);

        public DTTogglePanel(string[] names, Action<int> callback) => Prepare(18, names.Select(x => new GUIContent(x)).ToArray(), callback);

        public DTTogglePanel(string[] names, Action<int> callback, float height) => Prepare(height, names.Select(x => new GUIContent(x)).ToArray(), callback);

        public DTTogglePanel(string[] names, Action<int> callback, float height, int selected_panel) => Prepare(height, names.Select(x => new GUIContent(x)).ToArray(), callback, selected_panel);

        public DTTogglePanel(Texture[] textures, Action<int> callback, float height) => Prepare(height, textures.Select(x => new GUIContent(x)).ToArray(), callback);

        public DTTogglePanel(IconType[] icons, Action<int> callback, float height) => Prepare(height, icons.Select(x => new GUIContent(DTIcons.GetIcon(x))).ToArray(), callback);

        public DTTogglePanel(IconType[] icons, Action<int> callback, float height, int selected_panel) => Prepare(height, icons.Select(x => new GUIContent(DTIcons.GetIcon(x))).ToArray(), callback, selected_panel);

        public void AddButton(GUIContent content)
        {
            int id = Buttons.Count;
            DTToggleButton toggle;
            if (content.image == null)
            {
                toggle = new DTToggleButton(content.text, (val) => 
                { 
                    ListenButtonClicked(id); 
                });
            }
            else
            {
                toggle = new DTToggleButton(content.image, (val) => 
                { 
                    ListenButtonClicked(id); 
                });
            }

            toggle.Height = Height;
            if (content.image != null)
            {
                toggle.Width = Height;
            }
            Buttons.Add(toggle);
            Group.Add(toggle);
        }

        public int SelectedID()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Buttons[i].Pressed)
                {
                    return i;
                }
            }
            return 0;
        }

        protected virtual void ListenButtonClicked(int id)
        {
            var btn = Buttons[id];
            if (!btn.Pressed)
            {
                return;
            }
            OnSelectionChange?.Invoke(id);
        }

        protected override void AtDraw()
        {
            using (DTScope.HorizontalBox)
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].Draw();
                }
            }
        }

        private void Prepare(float height, GUIContent[] content, Action<int> callback, int selected_panel = 0)
        {
            Group = new DTToggleButtonGroup();
            this.Height = height;
            this.Buttons = new List<DTToggleButton>();
            if (callback != null)
            {
                OnSelectionChange += callback;
            }
            for (int i = 0; i < content.Length; i++)
            {
                AddButton(content[i]);
            }
            Buttons[selected_panel].SetPressed(true, false);
        }

    }
}