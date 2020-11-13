using UnityEngine;

public class LineCasting
{
    public static bool isLineClear(Vector3 point1, Vector3 point2, int ignoreLayers)
    {
        if (!Physics.Linecast(point1, point2, ~(1 << ignoreLayers)))
        {
            return true;
        }
        return false;
    }
}
