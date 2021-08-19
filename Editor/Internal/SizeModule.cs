using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DrawerTools
{
    public class SizeModule
    {
        public const float DEFAULT_H = 18f;
        public const float DEFAULT_W = 300f;

        private Vector2 size;
        private bool expands_width;
        private bool expands_height;
        private Dictionary<OptionType, GUILayoutOption> options_dict;

        public GUILayoutOption[] Options { get; protected set; }
        public bool ExpandsWidth { get => expands_width; set => ExpandWidth(value); }
        public bool ExpandsHeight { get => expands_height; set => ExpandHeight(value); }
        public float Height { get => size.y; set => SetHeight(value); }
        public float Width { get => size.x; set => SetWidth(value); }
        public Vector2 Size { get => size; set => SetSize(value); }

        public SizeModule()
        {
            size = new Vector2(Width, Height);
            expands_width = true;
            options_dict = new Dictionary<OptionType, GUILayoutOption>();
            Options = new GUILayoutOption[0];
        }

        public SizeModule SetSize(Vector2 size)
        {
            SetHeight(size.y);
            SetWidth(size.x);
            return this;
        }
        public SizeModule SetHeight(float value)
        {
            if (options_dict.ContainsKey(OptionType.CertainHeight))
            {
                options_dict.Remove(OptionType.CertainHeight);
            }
            options_dict.Add(OptionType.CertainHeight, GUILayout.Height(value));
            Options = options_dict.Values.ToArray();
            size.y = value;
            expands_height = false;
            return this;
        }
        public SizeModule SetWidth(float value)
        {
            if (options_dict.ContainsKey(OptionType.CertainWidth))
            {
                options_dict.Remove(OptionType.CertainWidth);
            }
            options_dict.Add(OptionType.CertainWidth, GUILayout.Width(value));

            Options = options_dict.Values.ToArray();
            size.x = value;
            expands_width = false;
            return this;
        }
        public SizeModule ExpandWidth(bool value)
        {
            if (!value)
            {
                SetWidth(size.x);
                return this;
            }

            if (options_dict.ContainsKey(OptionType.CertainWidth))
            {
                options_dict.Remove(OptionType.CertainWidth);
            }
            Options = options_dict.Values.ToArray();
            expands_width = true;
            return this;
        }
        public SizeModule ExpandHeight(bool value)
        {
            if (!value)
            {
                SetHeight(size.y);
                return this;
            }

            if (options_dict.ContainsKey(OptionType.CertainHeight))
            {
                options_dict.Remove(OptionType.CertainHeight);
            }
            Options = options_dict.Values.ToArray();
            expands_height = true;
            return this;
        }

        enum OptionType
        {
            CertainWidth,
            CertainHeight
        }
    }
}