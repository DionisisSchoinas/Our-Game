using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor (typeof(AttackIndicator))]
public class NewBehaviourScript : Editor
{
    void OnSceneGUI() 
    {
        AttackIndicator fow =(AttackIndicator)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position,Vector3.up, Vector3.forward,360,fow.attackRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.attackAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.attackAngle / 2, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.attackRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.attackRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}
