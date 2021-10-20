using System.Collections.Generic;
using System.Linq;

namespace DrawerTools
{
    public class DTToggleButtonGroup
    {
        public List<DTToggleButton> Group { get; protected set; }
        public DTToggleButton Selected { get; protected set; }
        public bool AllowNotSelected { get => allow_not_selected; set => SetAllowNotSelected(value); }

        private bool allow_not_selected = true;

        public DTToggleButtonGroup(params DTToggleButton[] arr)
        {
            Group = new List<DTToggleButton>(arr);

            var active = Group.Where(x => x.Pressed).ToList();
            if (active != null && active.Count() > 0)
            {
                Selected = active[0];
                active.RemoveAt(0);
                for (int i = 0; i < active.Count; i++)
                    active[i].SetPressed(false, false);
            }

            foreach (var item in arr)
            {
                item.OnPressedChanged += val => Switch(val, item);
            }
        }

        public void Select(int id, bool is_user_action)
        {
            if (Selected == Group[id])
            {
                return;
            }
            if (Selected != null)
            {
                Selected.SetPressed(false, is_user_action);
            }
            Selected = Group[id];
            Selected.SetPressed(true, is_user_action);
        }

        public void SetAllowNotSelected(bool allow)
        {
            allow_not_selected = allow;
            if (!allow && !Group.Any(x => x.Pressed))
            {
                Select(0, false);
            }
        }

        public void Add(DTToggleButton btn)
        {
            Group.Add(btn);
            btn.SetPressed(false, false);
            btn.OnPressedChanged += val => Switch(val, btn);
        }

        void Switch(bool val, DTToggleButton sender)
        {
            if (!val && Selected == sender)
            {
                Selected.SetPressed(false, false);
                Selected = null;
                return;
            }
            if (!val)
            {
                return;
            }

            if (Selected != null)
            {
                Selected.SetPressed(false, false);
            }
            Selected = sender;
        }
    }
}