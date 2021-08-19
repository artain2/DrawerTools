using UnityEngine;

namespace DrawerTools
{
    public interface ISize
    {
        float Height { get; set; }
        float Width { get; set; }
        Vector2 Size { get; set; }
    }
}