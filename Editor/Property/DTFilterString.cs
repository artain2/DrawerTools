using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawerTools
{
    public class DTFilterString<T> : DTDrawable where T : DTDrawable
    {
        public event Action<IEnumerable<T>> OnFilter;
        
        private DTString _stringField;
        private DTButton _clearBtn;
        private IEnumerable<T> _content;
        private Func<T, string> _selector;

        public bool AutoDisable = false;

        
        public DTFilterString(Func<T, string> selector, bool autoDisable = false)
        {
            _selector = selector;
            _stringField = new DTString("", "");
            _stringField.AddStringChangeCallback(AtInput);
            _clearBtn = new DTButton(FontIconType.Cross, () => _stringField.Value="");
            _clearBtn.SetWidth(20);
            AutoDisable = autoDisable;
        }
        
        public DTFilterString(IEnumerable<T> content, Func<T, string> selector, bool autoDisable = false) : this(selector, autoDisable)
        {
            SetContent(_content);
        }

        public DTFilterString(IEnumerable<T> content, Func<T, string> selector, Action<IEnumerable<T>> filterCallback) : this(content, selector, false)
        {
            OnFilter += filterCallback;
        }

        public DTFilterString<T>  SetContent(IEnumerable<T> content)
        {
            _content = content;
            return this;
        }

        private void AtInput(string filterStr)
        {
            if (_content == null)
                return;
            
            filterStr = filterStr.ToLower();
            var filtered = new List<T>();
            if (filterStr.IsNullOrEmpty())
            {
                filtered.AddRange(_content);
                AtFiltered(filtered);
                return;
            }
            var allMatches = _content
                .Where(x => _selector(x).ToLower().Contains(filterStr))
                .ToArray();
            var startsWith = allMatches
                .Where(x => _selector(x).ToLower().StartsWith(filterStr))
                .ToArray();
            var rest = allMatches
                .Except(startsWith);
            filtered.AddRange(startsWith);
            filtered.AddRange(rest);
            AtFiltered(filtered);
        }

        private void AtFiltered(List<T> filterResult)
        {
            if (AutoDisable)
            {
                var toDisable = _content.Except(filterResult);
                foreach (var item in toDisable)
                    item.SetActive(false);
                foreach (var item in filterResult)
                    item.SetActive(true);
            }
            OnFilter?.Invoke(filterResult);
        }

        protected override void AtDraw()
        {
            using (DTScope.Horizontal)
            {
                _stringField.Draw();
                _clearBtn.Draw();
            }
        }
    }
}