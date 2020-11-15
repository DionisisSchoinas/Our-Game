using UnityEngine;

public class LineCasting
{
    public static bool isLineClear(Vector3 point1, Vector3 point2, string[] ignoreLayers)
    {
        int ignore = LayerMask.GetMask(ignoreLayers);

        if (!Physics.Linecast(point1, point2, ~ignore))
        {
            return true;
        }
        return false;
    }
}
