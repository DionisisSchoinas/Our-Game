using UnityEngine;

public class LineCasting
{
    public static bool isLineClear(Vector3 point1, Vector3 point2, int ignoreLayersMask)
    {
        if (!Physics.Linecast(point1, point2, ~ignoreLayersMask))
        {
            return true;
        }
        return false;
    }
}
