using UnityEngine;

public static class ColliderExtensions 
{
    public static bool CheckIfPointIsInBounds(this Collider other, Vector3 point)
    {
        return other.bounds.Contains(point);
    }

}