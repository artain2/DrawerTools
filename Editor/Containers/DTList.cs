using System;
using System.Collections.Generic;

namespace DrawerTools
{
    public class DTList<T> : DTPanel where T : DTDrawable
    {
        public class Node : DTBase
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

        private DTButton addButton;
        private DTExpandToggle expandBtn = new DTExpandToggle();

        protected List<Node> nodesList = new List<Node>();
        private Func<T> _drawerCtor;

        public new string Name { get; set; }

        public DTList(IDTPanel parent) : base(parent)
        {
            addButton = new DTButton(FontIconType.Plus, AtAdd).SetRectSize(30) as DTButton;
        }

        public DTList<T> SetList(List<T> srcList, Func<T> drawerCtor)
        {
            nodesList.Clear();
            ItemsList = srcList;
            _drawerCtor = drawerCtor;
            foreach (var src in srcList)
            {
                nodesList.Add(CreateNode(src));
            }
            return this;
        }


        public new DTList<T> SetName(string name)
        {
            Name = name;
            return this;
        }

        protected override void AtDraw()
        {
            using (DTScope.Vertical)
            {
                using (DTScope.Horizontal)
                {
                    expandBtn.Draw();
                    DT.Label($"{Name} [{nodesList.Count}]");
                }

                if (expandBtn.Pressed)
                    DrawContent();
            }
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