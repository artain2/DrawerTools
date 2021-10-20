using System;
using System.Collections.Generic;

namespace DrawerTools
{
    public static class DTContainers
    {
        public static DTGrid CreateGrid(IDTPanel parent) => new DTGrid(parent);

        public static DTList<T, D> CreateList<T, D>(IDTPanel parent, Func<D> nodeConstructor, List<T> originalList) where T : new() where D : DTProperty
        {
            DTList<T, D> list = null;
            if (typeof(T).IsClass)
            {
                list = new DTClassPropertyList<T, D>(parent, nodeConstructor);
            }
            else
            {
                list = new DTStructPropertyList<T, D>(parent, nodeConstructor);
            }
            list.SetList(originalList);
            return list;
        }

        public class DTClassPropertyList<T, D> : DTList<T, D> where T : new() where D : DTProperty
        {
            public DTClassPropertyList(IDTPanel parent, Func<D> nodeConstructor) : base(parent, nodeConstructor) { }
        }
        public class DTStructPropertyList<T, D> : DTList<T, D> where T : new() where D : DTProperty
        {
            public DTStructPropertyList(IDTPanel parent, Func<D> nodeConstructor) : base(parent, nodeConstructor) { }

            protected override void AtNodeValueChanged(Node sender)
            {
                ValuesList[sender.ID] = sender.Value;
            }
        }
    }
}