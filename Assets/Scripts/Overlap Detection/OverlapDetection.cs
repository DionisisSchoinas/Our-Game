using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapDetection : MonoBehaviour
{
    public static GameObject[] NoObstaclesLine(Collider[] colliders, Vector3 centerPoint, int ignoreLayersMask)
    {
        List<GameObject> gms = new List<GameObject>();
        foreach(Collider collider in colliders)
        {
            if (!gms.Contains(collider.gameObject))
            {
                if (LineCasting.isLineClear(collider.transform.position, centerPoint, ignoreLayersMask))
                {
                    gms.Add(collider.gameObject);
                }
            }
        }
        return gms.ToArray();
    }
    public static GameObject[] NoObstaclesVertical(Collider[] colliders, Vector3 centerPoint, int ignoreLayersMask)
    {
        List<GameObject> gms = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            if (!gms.Contains(collider.gameObject))
            {
                Vector3 belowEntity = new Vector3(collider.transform.position.x, centerPoint.y, collider.transform.position.z);
                if (LineCasting.isLineClear(collider.transform.position, belowEntity, ignoreLayersMask))
                {
                    gms.Add(collider.gameObject);
                }
            }
        }
        return gms.ToArray();
    }
    public static GameObject[] NoObstaclesHorizontal(Collider[] colliders, Vector3 centerPoint, int ignoreLayersMask)
    {
        List<GameObject> gms = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            if (!gms.Contains(collider.gameObject))
            {
                Vector3 sameHeight = new Vector3(centerPoint.x, collider.transform.position.y, centerPoint.z);
                if (LineCasting.isLineClear(collider.transform.position, sameHeight, ignoreLayersMask))
                {
                    gms.Add(collider.gameObject);
                }
            }
        }
        return gms.ToArray();
    }
}
