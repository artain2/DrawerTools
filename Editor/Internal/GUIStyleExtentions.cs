using System.Collections.Generic;
using UnityEngine;

namespace DrawerTools
{
    public static class GUIStyleExtentions
    {
        public static List<GUIStyleState> GetAllStates(this GUIStyle style) => 
            new List<GUIStyleState>(new GUIStyleState[] { style.active, style.normal, style.hover, style.focused });
    }
}