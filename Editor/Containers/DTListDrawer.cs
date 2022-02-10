using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DrawerTools
{
    public class DTListDrawer<T> : DTPanel where T : DTDrawable
    {
        private class Node : DTBase
        {
            public T Item { get; private set; }
            public DTButton RemoveButton { get; private set; }
            public DTButton UpButton { get; private set; }

            public Node(T value, Action<Node> atRemove, Action<Node> atUp)
            {
                Item = value;
                RemoveButton = new DTButton(FontIconType.Fire, () => atRemove(this)).SetRectSize(20) as DTButton;
                UpButton = new DTButton(FontIconType.StepUp, () => atUp(this)).SetRectSize(20) as DTButton;
            }

            protected override void AtDraw()
            {
                using (DTScope.Horizontal)
                {
                    UpButton.Draw();
                    RemoveButton.Draw();

                    using (DTScope.Vertical)
                    {
                        Item.Draw();
                    }
                }
            }
        }

        public event Action OnListChange;
        public List<T> ItemsList { get; private set; }
        public new string Name { get; set; }
        public bool ShowAmountInTitle { get; set; } = true;


        private DTButton addButton;
        private DTExpandToggle expandBtn = new DTExpandToggle();

        private List<Node> nodesList = new List<Node>();
        private Func<T> _drawerCtor;


        #region DT

        public DTListDrawer(IDTPanel parent) : base(parent)
        {
            addButton = new DTButton(FontIconType.Plus, AtAdd).SetRectSize(30) as DTButton;
        }
        
        protected override void AtDraw()
        {
            using (DTScope.Vertical)
            {
                using (DTScope.Horizontal)
                {
                    expandBtn.Draw();
                    var titleStr = Name;
                    if (ShowAmountInTitle)
                        titleStr += $" [{nodesList.Count}]";
                    DT.Label(titleStr);
                }

                if (expandBtn.Pressed)
                    DrawContent();
            }
        }

        #endregion

        public DTListDrawer<T> SetList(List<T> srcList, Func<T> drawerCtor)
        {
            SetList(srcList);
            SetItemCreateDelegate(drawerCtor);
            SetAddEnable(true);
            return this;
        }
      
        public DTListDrawer<T> SetList(List<T> srcList)
        {
            nodesList.Clear();
            ItemsList = srcList;
            SetAddEnable(false);
            foreach (var src in srcList)
            {
                nodesList.Add(CreateNode(src));
            }
            return this;
        }

        public new DTListDrawer<T> SetName(string name)
        {
            Name = name;
            return this;
        }

        public void SetItemCreateDelegate(Func<T> drawerCtor)
        {
            _drawerCtor = drawerCtor;
        }
        
        public DTListDrawer<T> SetAddEnable(bool enable)
        {
            addButton.SetActive(enable);
            return this;
        }

        public DTListDrawer<T> SetRemoveEnable(bool enable)
        {
            foreach (var node in nodesList)
            {
                node.RemoveButton.SetActive(enable);
            }

            return this;
        }

        
        private void DrawContent()
        {
            DTScope.Begin(Scope.HorizontalOffset);
            for (int i = 0; i < nodesList.Count; i++)
                nodesList[i].Draw();
            DTScope.End(Scope.HorizontalOffset);
            addButton.Draw();
        }

        private void AtMoveUp(Node node)
        {
            int id = nodesList.IndexOf(node);
            if (id == 0)
                return;

            var tmpVal = nodesList[id - 1];
            nodesList[id - 1] = node;
            nodesList[id] = tmpVal;

            var tmpSrc = ItemsList[id - 1];
            ItemsList[id - 1] = node.Item;
            ItemsList[id] = tmpSrc;
            OnListChange?.Invoke();
        }

        private void AtRemove(Node node)
        {
            int id = nodesList.IndexOf(node);
            nodesList.RemoveAt(id);
            ItemsList.RemoveAt(id);
            OnListChange?.Invoke();
        }

        private Node CreateNode(T item)
        {
            var node = new Node(item, AtRemove, AtMoveUp);
            return node;
        }

        private void AtAdd()
        {
            var created = _drawerCtor();
            ItemsList.Add(created);
            nodesList.Add(CreateNode(created));
            OnListChange?.Invoke();
        }
    }
}