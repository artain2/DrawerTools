using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DrawerTools
{
    public class DTGrid : DTPanel
    {
        private Vector2 itemSize = new Vector2(40, 40);
        private Vector2 spacing = new Vector2(0, 0);
        private Vector2 bordersMin = new Vector2(0, 0);
        private Vector2 bordersMax = new Vector2(0, 0);
        private List<DTDrawable> items = new List<DTDrawable>();

        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public Vector2 ItemSize { get => itemSize; set => SetItemSize(value.x, value.y); }
        public float MaxWidth => GetFixedSize().x;
        private Vector2 UnityItemSize => itemSize + Vector2.one * DT.UNITY_SPACING;

        public DTGrid(IDTPanel parent) : base(parent) { }

        public DTGrid AddItem<T>(T item) where T : DTDrawable => AddItems(item);

        public DTGrid AddItems<T>(params T[] items) where T : DTDrawable => AddItems(items as IList<T>);

        public DTGrid AddItems<T>(IList<T> items) where T : DTDrawable
        {
            foreach (var item in items)
            {
                this.items.Add(item);
                item.SetSize(itemSize);
            }
            AtSizeChanged();
            return this;
        }

        public void SetItemAsLast<T>(T item) where T : DTDrawable
        {
            items.Remove(item);
            items.Add(item);
            AtSizeChanged();
        }

        public void SetItemAsFirst<T>(T item) where T : DTDrawable
        {
            items.Remove(item);
            items.Insert(0, item);
            AtSizeChanged();
        }

        public DTGrid SetSpacing(float? x = null, float? y = null)
        {
            if (x.HasValue)
            {
                spacing.x = x.Value;
            }
            if (y.HasValue)
            {
                spacing.y = y.Value;
            }
            AtSizeChanged();
            return this;
        }

        public DTGrid SetBorders(float? xMin = null, float? yMin = null, float? xMax = null, float? yMax = null)
        {
            if (xMin.HasValue)
            {
                bordersMin.x = xMin.Value;
            }
            if (yMin.HasValue)
            {
                bordersMin.y = yMin.Value;
            }
            if (xMax.HasValue)
            {
                bordersMax.x = xMax.Value;
            }
            if (yMax.HasValue)
            {
                bordersMax.y = yMax.Value;
            }
            AtSizeChanged();
            return this;
        }

        public DTGrid SetItemSize(float? x = null, float? y = null)
        {
            if (x.HasValue)
            {
                itemSize.x = x.Value;
            }
            if (y.HasValue)
            {
                itemSize.y = y.Value;
            }
            items.ForEach(item => item.SetSize(itemSize));
            AtSizeChanged();
            return this;
        }

        protected override void AtSizeChanged()
        {
            Columns = Mathf.FloorToInt((MaxWidth - UnityItemSize.x - bordersMin.x - bordersMax.x - DT.UNITY_SPACING) / (UnityItemSize.x + spacing.x)) + 1;
            Rows = Mathf.CeilToInt(items.Where(x => x.Active).Count() / (float)Columns);
            base.AtSizeChanged();

        }

        protected override void AtDraw()
        {
            DT.Space(bordersMin.y);
            if (bordersMin.x > 0)
            {
                DTScope.BeginHorizontalOffset(bordersMin.x - DT.UNITY_SPACING);
            }
            int iter = -1;
            DTScope.Begin(Scope.Vertical);
            for (int i = 0; i < Rows; i++)
            {
                int itemsInThisRow = Mathf.Min(Columns, items.Count - i * Columns);
                DTScope.Begin(Scope.Horizontal);
                for (int j = 0; j < itemsInThisRow - 1; j++)
                {
                    items[++iter].Draw();
                    DT.Space(spacing.x);
                }
                items[++iter].Draw();
                DT.Space(bordersMax.x);
                DTScope.End(Scope.Horizontal);
                DT.Space(spacing.y);
            }
            if (bordersMax.y > 0)
            {
                DT.Space(bordersMax.y);
            }
            DTScope.End(Scope.Vertical);
            if (bordersMin.x > 0)
            {
                DTScope.EndHorizontalOffset();
            }
        }
    }
}