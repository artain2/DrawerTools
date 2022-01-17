using System;
using System.Collections.Generic;

namespace DrawerTools
{
    public static class DTContainers
    {
        public static DTGrid CreateGrid(IDTPanel parent) => new DTGrid(parent);

    }
}