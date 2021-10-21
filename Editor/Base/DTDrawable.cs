using System;
using UnityEngine;

namespace DrawerTools
{

    /// <summary>
    /// Size controll class
    /// Adds:
    ///     <see cref="OnSizeChange"/> 
    ///     <see cref="Sizer"/> - Expands size by parent or set fixed size
    ///     <see cref="Size"/> etc. - fast access to sizer methods
    ///     <see cref="Name"/> allows to set name or texture
    /// Realize: 
    ///     <see cref="ISize"/>
    /// </summary>
    public abstract class DTDrawable : DTBase, ISize
    {
        public event Action OnSizeChange;

        public SizeModule Sizer { get; protected set; } = new SizeModule();
        public virtual float Height { get => Sizer.Height; set => SetHeight(value); }
        public virtual float Width { get => Sizer.Width; set => SetWidth(value); }
        public virtual Vector2 Size { get => Sizer.Size; set => SetSize(value); }
        public virtual float RectSize { set => SetRectSize(value); }
        public string Name { get => content.text; set => SetName(value); }
        public string Tooltip { get => content.tooltip; set => content.tooltip = value; }
        public Texture Icon { get => content.image; set => content = new GUIContent(value, Tooltip); }

        public DTDrawable SetRectSize(float size)
        {
            Sizer.Height = size;
            Sizer.Width = size;
            AtSizeChanged();
            return this;
        }
        public DTDrawable SetWidth(float size)
        {
            Sizer.Width = size;
            AtSizeChanged();
            return this;
        }
        public DTDrawable SetHeight(float size)
        {
            Sizer.Height = size;
            AtSizeChanged();
            return this;
        }
        public DTDrawable SetSize(Vector2 size)
        {
            Sizer.Size = size;
            AtSizeChanged();
            return this;
        }
        public DTDrawable SetSize(float x, float y) => SetSize(new Vector2(x, y));

        public virtual DTDrawable SetName(string name)
        {
            content = new GUIContent(name, Tooltip);
            return this;
        }

        protected virtual void AtSizeChanged()
        {
            OnSizeChange?.Invoke();
        }

        protected GUIContent content = new GUIContent("");
        public DTDrawable() => Name = "";
        public DTDrawable(Texture tex, string tooltip = null)
        {
            Icon = tex;
            Tooltip = tooltip;
        }
        public DTDrawable(string text, string tooltip = null)
        {
            Name = text;
            Tooltip = tooltip;
        }
    }
}