using System;
using UnityEngine;

namespace DrawerTools
{
    /// <summary>
    /// Base panel class
    /// Realize: some <see cref="IDTPanel"/> behaviours
    /// </summary>
    public abstract class DTPanel : DTDrawable, IDTPanel
    {
        public class DefaultPanelBehaviour<T> where T : DTDrawable, IDTPanel
        {
            private T target;

            protected Scroll scroll = new Scroll();

            public IDTPanel Parent { get; set; }
            public bool DrawScrollInExpand { get; set; } = false;
            public bool DrawName { get; set; } = true;


            public DefaultPanelBehaviour(T _target, IDTPanel parent, Action atParentSizeChanged)
            {
                target = _target;
                target.Sizer.ExpandsHeight = true;
                Parent = parent;
                scroll.SetSizer(target.Sizer);
                Parent.OnSizeChange += atParentSizeChanged;
                target.OnBeforeDraw += BeforeDraw;
                target.OnAfterDraw += AfterDraw;
            }

            public Vector2 GetFixedSize(float? x = null, float? y = null)
            {
                if (!x.HasValue && !target.Sizer.ExpandsWidth)
                    x = target.Sizer.Width;
                if (!y.HasValue && !target.Sizer.ExpandsHeight)
                    y = target.Sizer.Width;
                if (x.HasValue && y.HasValue)
                    return new Vector2(x.Value, y.Value);
                return Parent.GetFixedSize(x, y);
            }

            protected void BeforeDraw()
            {
                if (target.Name != "" && DrawName)
                    DT.Label(target.Name);
                if (!target.Sizer.ExpandsHeight || DrawScrollInExpand)
                    scroll.Begin();
            }

            protected void AfterDraw()
            {
                if (!target.Sizer.ExpandsHeight || DrawScrollInExpand)
                    scroll.End();
            }
        }

        protected DefaultPanelBehaviour<DTPanel> panelBeh;

        public DTPanel(IDTPanel parent) : base()
        {
            panelBeh = new DefaultPanelBehaviour<DTPanel>(this, parent, AtSizeChanged);
        }

        public IDTPanel Parent => panelBeh.Parent;

        public Vector2 GetFixedSize(float? x = null, float? y = null) => panelBeh.GetFixedSize(x, y);

        public DTPanel SetExpandable(bool expandable)
        {
            panelBeh.DrawScrollInExpand = expandable;
            return this;
        }
    }
}