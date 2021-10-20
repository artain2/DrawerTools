using System;
using System.Collections.Generic;

namespace DrawerTools
{
    public abstract class DTList<T, D> : DTPanel where T : new() where D : DTProperty
    {
        public class Node : DTBase
        {
            public event Action<Node> OnValueChange;

            private Func<D> nodeConstructor;

            public int ID { get; set; }
            public T Value { get; private set; }
            public D Drawer { get; private set; }
            public DTButton RemoveButton { get; private set; }
            public DTButton UpButton { get; private set; }

            public Node(T value, Action<Node> atRemove, Action<Node> atUp, Func<D> nodeConstructor)
            {
                this.nodeConstructor = nodeConstructor;
                Value = value;
                Drawer = nodeConstructor();
                Drawer.UncastedValue = value;
                Drawer.OnValueChanged += AtDrawerValueChange;
                RemoveButton = new DTButton(FontIconType.Fire, () => atRemove(this)).SetRectSize(20) as DTButton;
                UpButton = new DTButton(FontIconType.StepUp, () => atUp(this)).SetRectSize(20) as DTButton;
            }

            private void AtDrawerValueChange()
            {
                Value = (T)Drawer.UncastedValue;
                OnValueChange?.Invoke(this);
            }

            public Node SetID(int id)
            {
                ID = id;
                return this;
            }

            protected override void AtDraw()
            {
                DTScope.Begin(Scope.Horizontal);
                UpButton.Draw();
                RemoveButton.Draw();

                DTScope.Begin(Scope.Vertical);
                Drawer.Draw();
                DTScope.End(Scope.Vertical);

                DTScope.End(Scope.Horizontal);
            }
        }

        private DTButton addButton;
        private DTExpandToggle expandBtn = new DTExpandToggle();

        protected List<Node> nodesList;
        protected Func<D> nodeConstructor;

        public List<T> ValuesList { get; private set; }
        public Func<T> ItemConstructor { get; set; } = () => new T();
        public new string Name { get; set; }

        public DTList(IDTPanel parent, Func<D> nodeConstructor) : base(parent)
        {
            this.nodeConstructor = nodeConstructor;
            addButton = new DTButton(FontIconType.Plus, AtAdd).SetRectSize(30) as DTButton;
        }

        public DTList<T, D> SetList(List<T> originalList)
        {
            ValuesList = originalList;
            Repaint();
            return this;
        }

        public void Repaint()
        {
            nodesList = new List<Node>();
            for (int i = 0; i < ValuesList.Count; i++)
                AddNode(i);
        }

        public DTList<T, D> SetItemConstructor(Func<T> ctor)
        {
            ItemConstructor = ctor;
            return this;
        }

        public new DTList<T, D> SetName(string name)
        {
            Name = name;
            return this;
        }

        protected override void AtDraw()
        {
            DTScope.Begin(Scope.Vertical);

            // Title
            DTScope.Begin(Scope.Horizontal);
            expandBtn.Draw();
            DT.Label($"{Name} [{ValuesList.Count}]");
            DTScope.End(Scope.Horizontal);

            if (expandBtn.Pressed)
                DrawContent();

            DTScope.End(Scope.Vertical);
        }

        protected virtual void AtNodeValueChanged(Node sender) { }

        private void DrawContent()
        {
            DTScope.Begin(Scope.HorizontalOffset);
            for (int i = 0; i < nodesList.Count; i++)
                nodesList[i].Draw();
            DTScope.End(Scope.HorizontalOffset);
            addButton.Draw();
        }

        private void AddNode(int id)
        {
            var node = new Node(ValuesList[id], AtRemove, AtMoveUp, nodeConstructor).SetID(id);
            node.OnValueChange += AtNodeValueChanged;
            nodesList.Add(node);
        }

        private void AtMoveUp(Node node)
        {
            int id = node.ID;
            if (id == 0)
                return;

            var tmpVal = ValuesList[id - 1];
            ValuesList.RemoveAt(id - 1);
            ValuesList.Insert(id, tmpVal);

            var tmpNode = nodesList[id - 1];
            nodesList.RemoveAt(id - 1);
            nodesList.Insert(id, tmpNode);

            ValidateID();
        }

        private void AtRemove(Node node)
        {
            ValuesList.RemoveAt(node.ID);
            nodesList.RemoveAt(node.ID);
            ValidateID();
        }

        private void AtAdd()
        {
            var created = ItemConstructor();
            ValuesList.Add(created);
            AddNode(ValuesList.Count - 1);
        }

        private void ValidateID()
        {
            for (int i = 0; i < ValuesList.Count; i++)
                nodesList[i].SetID(i);
        }
    }
}