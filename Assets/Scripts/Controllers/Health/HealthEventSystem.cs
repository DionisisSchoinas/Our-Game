using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class HealthEventSystem : MonoBehaviour
{
    // Job for health control
    public struct HealthDamageJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<int> objectNameIds;
        [ReadOnly] public int beingDamagedNameId;
        [ReadOnly] public float damageValue;
        [ReadOnly] public int damageType;
        [ReadOnly] public NativeArray<HealthController.HealthControllerData> data;
        public NativeArray<float> health;
        [ReadOnly] public NativeArray<int> resistancesIndex; // Contains the ening index of each controller's data in the "resistances" array
        [ReadOnly] public NativeArray<int> resistances;  // Contains the resistances of all the controllers
        [ReadOnly] public NativeArray<int> immunitiesIndex; // Contains the ening index of each controller's data in the "immunities" array
        [ReadOnly] public NativeArray<int> immunities;  // Contains the immunities of all the controllers

        public void Execute(int index)
        {
            // If controller matches
            if (objectNameIds[index] == beingDamagedNameId)
            {
                // If controller not invulnerable
                if (!data[index].invulnerable)
                {
                    health[index] = data[index].health - CheckDamageTypes(index);
                }
            }
        }

        private float CheckDamageTypes(int index)
        {
            // Gets the indexies to be searched in the total immunity array
            int immStartIndex = (index == 0) ? 0 : immunitiesIndex[index - 1];
            int immEndIndex = immunitiesIndex[index];

            // Gets the indexies to be searched in the total resistance array
            int resStartIndex = (index == 0) ? 0 : resistancesIndex[index - 1];
            int resEndIndex = resistancesIndex[index];

            for (int i = immStartIndex; i < immEndIndex; i++)
            {
                if (immunities[i] == damageType)
                {
                    return 0f;
                }
            }

            for (int i = resStartIndex; i < resEndIndex; i++)
            {
                if (resistances[i] == damageType)
                {
                    return damageValue / 2f;
                }
            }
            
            return damageValue;
        }
    }

    public static HealthEventSystem current;

    private List<HealthController> healthControllers;
    private Dictionary<string, int> healthControllersNames;
    private NativeList<int> idsArray;

    private int ids;


    private void Awake()
    {
        current = this;

        // Instatiate lists and native arrays
        healthControllers = new List<HealthController>();
        healthControllersNames = new Dictionary<string, int>();
        idsArray = new NativeList<int>(Allocator.Persistent);

        ids = 0;
    }

    private void OnDestroy()
    {
        // Clear memory
        idsArray.Dispose();
    }

    // Deals damage ignoring invunarable
    public void TakeDamage(string name, float damage, int damageType)
    {
        // Copy data to Tmp native array
        NativeArray<HealthController.HealthControllerData> dataTmp = new NativeArray<HealthController.HealthControllerData>(healthControllers.Count, Allocator.TempJob);
        NativeArray<float> health = new NativeArray<float>(healthControllers.Count, Allocator.TempJob);

        // Index ends of both arrays
        // valeus inside like -> [2, 2, 3, 4] when the 1st controller has 2 entries, the 2nd 0, the 3rd 1 and the 4rth 1
        NativeArray<int> resistancesIndex = new NativeArray<int>(healthControllers.Count, Allocator.TempJob);
        NativeArray<int> immunitiesIndex = new NativeArray<int>(healthControllers.Count, Allocator.TempJob);

        // Contnious arrays of values
        NativeList<int> resistances = new NativeList<int>(Allocator.TempJob);
        NativeList<int> immunities = new NativeList<int>(Allocator.TempJob);

        for (int i=0; i<healthControllers.Count; i++)
        {
            dataTmp[i] = healthControllers[i].data;
            health[i] = healthControllers[i].data.health;
            resistancesIndex[i] = healthControllers[i].resistances.Count + ((i == 0) ? 0 : resistancesIndex[i-1]);
            immunitiesIndex[i] = healthControllers[i].immunities.Count + ((i == 0) ? 0 : immunitiesIndex[i-1]);

            foreach (int type in healthControllers[i].resistances)
                resistances.Add(type);

            foreach (int type in healthControllers[i].immunities)
                immunities.Add(type);
        }

        // Check if given name is a controller
        int id;
        if (!healthControllersNames.TryGetValue(name, out id))
            id = -10;

        // Create Job object
        HealthDamageJob healthDamageJob = new HealthDamageJob()
        {
            damageValue = damage,
            damageType = damageType,
            beingDamagedNameId = id,
            objectNameIds = idsArray,
            data = dataTmp,
            health = health,
            resistancesIndex = resistancesIndex,
            resistances = resistances,
            immunitiesIndex = immunitiesIndex,
            immunities = immunities,
        };

        // Start Jobs
        JobHandle jobHandle = healthDamageJob.Schedule(healthControllers.Count, 1);
        jobHandle.Complete();

        // Transfer new data over to the actual data
        for (int i = 0; i < healthControllers.Count; i++)
        {
            healthControllers[i].currentValue = health[i];
        }

        // Clear memory
        dataTmp.Dispose();
        health.Dispose();
        resistancesIndex.Dispose();
        resistances.Dispose();
        immunitiesIndex.Dispose();
        immunities.Dispose();
    }

    // Subscribes a healthcontroller to the evetn system and returns the ID of the controller
    public int Subscribe(HealthController healthController)
    {
        // Add controller to the lists
        ids++;
        healthControllers.Add(healthController);
        healthControllersNames.Add(healthController.gameObject.name, ids);

        // Add controller data to the Job's NativeArrays
        if (idsArray.IsCreated)
            idsArray.Add(ids);

        return ids;
    }

    // Unsubscribes a healthcontroller from the evetn system
    public void UnSubscribe(HealthController healthController)
    {
        // Remove controller data from the lists
        healthControllers.Remove(healthController);
        healthControllersNames.Remove(healthController.gameObject.name);

        // Remove controller data from the Job's NativeArrays
        if (idsArray.IsCreated && idsArray.IndexOf(healthController.healthSystemId) >= 0)
            idsArray.RemoveAt(idsArray.IndexOf(healthController.healthSystemId));
    }

    // Sets the invunarablility state
    public event Action<string, bool> onChangeInvunerability;
    public void SetInvunerable(string name, bool state)
    {
        if (onChangeInvunerability != null)
        {
            onChangeInvunerability(name, state);
        }
    }
    // Applies a condition
    public event Action<string, Condition> onConditionHit;
    public void SetCondition(string name, Condition condition)
    {
        if (onConditionHit != null)
        {
            onConditionHit(name, condition);
        }
    }
    // Applies force
    public event Action<string, Vector3, float> onForceApply;
    public void ApplyForce(string name, Vector3 direction, float magnitude)
    {
        if (onForceApply != null)
        {
            onForceApply(name, direction.normalized, magnitude);
        }
    }
    // Applies resistance
    public event Action<string, SkinnedMeshRenderer, Material, int, float> onResistanceApply;
    public void ApplyResistance(string name, SkinnedMeshRenderer mesh, Material newMaterial, int resistance, float duration)
    {
        if (onResistanceApply != null)
        {
            onResistanceApply(name, mesh, newMaterial, resistance, duration);
        }
    }
    // Sends out current resistances
    public event Action<string, List<int>> onResistanceUpdate;
    public void UpdateResistance(string name, List<int> resistances)
    {
        if (onResistanceUpdate != null)
        {
            onResistanceUpdate(name, resistances);
        }
    }
}
