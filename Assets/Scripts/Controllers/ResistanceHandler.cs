using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResistanceHandler : MonoBehaviour
{
    [HideInInspector]
    public List<int> damageResistances;
    [HideInInspector]
    public List<int> damageImmunities;

    private Coroutine resistanceTimer;
    private bool countingResistanceDuration;

    private void Start()
    {
        damageResistances = new List<int>();
        damageImmunities = new List<int>();

        HealthEventSystem.current.onResistanceApply += ApplyResistance;
    }

    private void OnDestroy()
    {
        HealthEventSystem.current.onResistanceApply -= ApplyResistance;
    }

    //-------------- Resistance Management --------------
    public void ApplyResistance(string name, SkinnedMeshRenderer mesh, Material newMaterial, int resistance, float duration)
    {
        if (gameObject.name != name)
            return;

        RemoveResistance(mesh);
        AddResistance(mesh, newMaterial, resistance, duration);
    }

    private void AddResistance(SkinnedMeshRenderer mesh, Material newMaterial, int resistance, float duration)
    {
        List<Material> mats = mesh.materials.ToList();
        mats.Add(newMaterial);
        mesh.materials = mats.ToArray<Material>(); // Adds the resistance material to the mesh

        damageResistances.Add(resistance); // Add resistance to list

        resistanceTimer = StartCoroutine(StartDuration(mesh, duration));
    }

    private void RemoveResistance(SkinnedMeshRenderer mesh)
    {
        if (!countingResistanceDuration)
            return;

        StopCoroutine(resistanceTimer); //Stops coroutine counting duration

        List<Material> mats = mesh.materials.ToList();
        mats.RemoveAt(mats.Count - 1);
        mesh.materials = mats.ToArray<Material>(); // Takes entire array except the last element

        damageResistances.Clear(); // Empty resistance list ( works since we only have 1 way to add resistances )
    }

    private IEnumerator StartDuration(SkinnedMeshRenderer mesh, float duration)
    {
        countingResistanceDuration = true;
        yield return new WaitForSeconds(duration);
        RemoveResistance(mesh);
        countingResistanceDuration = false;
    }
}