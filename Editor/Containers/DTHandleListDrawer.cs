using System;
using System.Collections.Generic;
using DrawerTools.Internal;

namespace DrawerTools
{
    public class DTListDrawer<TSource, TDrawer> : DTPanel
        where TDrawer : DTDrawable
    {
        private List<TSource> _srcListHandle;
        private List<Node> _nodes = new List<Node>();
        private Func<TSource, TDrawer> _drawerCtor;
        private Func<TSource> _srcCtor;
        
        private Title _title = new Title();
        private DTButton _addButton;
        private bool _isRemoveEnable = true;
        private Node _movingNode = null;

        public DTListDrawer(IDTPanel parent) : base(parent)
        {     
            _addButton = new DTButton(FontIconType.Plus, AtAdd)
                .SetRectSize(30).SetActive(false) as DTButton;
        }
        
        protected override void AtDraw()
        {
            _title.Draw();
            if (!_title.Opened)
                return;
            DTScope.BeginHorizontalOffset(25);
            for (var i = 0; i < _nodes.Count; i++)
                _nodes[i].Draw();
            DTScope.EndHorizontalOffset();
        }

        public void SetList(List<TSource> srcList, Func<TSource, TDrawer> drawerCtor)
        {
            _srcListHandle = srcList;
            _drawerCtor = drawerCtor;
            _nodes = new List<Node>();
            foreach (var src in _srcListHandle)
                _nodes.Add(CreateNode(src));
        }
        
        public void AllowTitle(string title)
        {
            _title.SetActive(true);
            _title.Text = title;
        }

        public void DisableTitle()
        {
            _title.SetActive(false);
            _title.Opened = true;
        }

        public void SetRemoveEnabled(bool enable)
        {
            _isRemoveEnable = enable;
            foreach (var node in _nodes)
            {
                node.RemoveButton.SetActive(enable);
            }
        }


        public void AllowAdd(Func<TSource> srcCtor)
        {
            _srcCtor = srcCtor;
            _addButton.SetActive(true);
        }

        public void DisableAdd()
        {
            _addButton.SetActive(false);
        }

        private void AtMoveClick(Node node)
        {
            if (_movingNode == null)
            {
                _movingNode = node;
                foreach (var n in _nodes)
                    n.SetMove(MoveType.In);
                node.SetMove(MoveType.Disable);
                return;
            }
            
            var a = _nodes.IndexOf(node);
            var b = _nodes.IndexOf(_movingNode);

            (_nodes[a], _nodes[b]) = (_nodes[b], _nodes[a]);
            (_srcListHandle[a], _srcListHandle[b]) = (_srcListHandle[b], _srcListHandle[a]);
            _movingNode = null;
            foreach (var n in _nodes)
                n.SetMove(MoveType.Out);
        }

        private void AtRemove(Node node)
        {
            var id = _nodes.IndexOf(node);
            _nodes.RemoveAt(id);
            _srcListHandle.RemoveAt(id);
        }
        
        private void AtAdd()
        {
            var src = _srcCtor();
            _srcListHandle.Add(src);
            
            _nodes.Add(CreateNode(src));
        }

        private Node CreateNode(TSource src)
        {
            var node = new Node(AtRemove, AtMoveClick)
            {
                Source = src,
                Drawer = _drawerCtor(src)
            };
            node.RemoveButton.SetActive(_isRemoveEnable);
            return node;
        }

        private class Node : DTDrawable
        {
            public TSource Source;
            public TDrawer Drawer;

            public DTButton RemoveButton { get; private set; }
            public DTButton MoveButton { get; private set; }


            public Node(Action<Node> atRemove, Action<Node> atMove)
            {
                RemoveButton = new DTButton(FontIconType.Cross, () => atRemove(this)).SetRectSize(20) as DTButton;
                MoveButton = new DTButton(FontIconType.OutBox, () => atMove(this)).SetRectSize(23) as DTButton;
            }

            protected override void AtDraw()
            {
                using (DTScope.Horizontal)
                {
                    MoveButton.Draw();
                    RemoveButton.Draw();

                    using (DTScope.Vertical)
                    {
                        Drawer.Draw();
                    }
                }
            }

            public void SetMove(MoveType moveType)
            {
                MoveButton.Disabled = moveType == MoveType.Disable;
                MoveButton.SetFontIcon(moveType == MoveType.In ? FontIconType.InBox : FontIconType.OutBox, true);
            }
        }
        
        private enum MoveType
        {
            Out,
            In,
            Disable,
        }
    }
}