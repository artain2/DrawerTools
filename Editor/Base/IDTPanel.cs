using System;
using UnityEngine;

namespace DrawerTools
{
    public interface IDTPanel
    {
        event Action OnSizeChange;
        IDTPanel Parent { get; }
        Vector2 GetFixedSize(float? x = null, float? y = null);
    }
}