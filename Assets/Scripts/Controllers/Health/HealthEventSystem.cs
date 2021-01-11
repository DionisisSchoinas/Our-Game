using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class HealthEventSystem : MonoBehaviour
{

    public struct HealthDamageJob : IJobParallelFor
    {
        public NativeArray<int> objectNameIndex;
        public int beingDamagedNameIndex;
        public float damageValue;
        public NativeArray<float> currenthealth;

        public void Execute(int index)
        {
            if (objectNameIndex[index] == beingDamagedNameIndex)
            {
                currenthealth[index] = currenthealth[index] - damageValue;
            }
        }
    }

    public static HealthEventSystem current;

    private List<HealthController> healthControllers;
    private Dictionary<string, int> healthControllersNames;
    private NativeArray<int> idsArray;

    private int ids;


    private void Awake()
    {
        current = this;
        healthControllers = new List<HealthController>();
        healthControllersNames = new Dictionary<string, int>();
        idsArray = new NativeArray<int>(0, Allocator.Persistent); ;
        ids = 0;
    }

    private void OnDestroy()
    {
        idsArray.Dispose();
    }

    // Deals damage ignoring invunarable
    public event Action<string, float, int> onDamageTaken;
    public void TakeDamage(string name, float damage, int damageType)
    {
        /*
        if (onDamageTaken != null)
        {
            onDamageTaken(name, damage, damageType);
        }
        */
        NativeArray<float> currentHealth = new NativeArray<float>(healthControllers.Count, Allocator.TempJob);

        for (int i=0; i<healthControllers.Count; i++)
        {
            currentHealth[i] = healthControllers[i].currentValue;
        }

        HealthDamageJob healthDamageJob = new HealthDamageJob()
        {
            damageValue = damage,
            beingDamagedNameIndex = healthControllersNames[name],
            objectNameIndex = idsArray,
            currenthealth = currentHealth,
        };

        JobHandle jobHandle = healthDamageJob.Schedule(healthControllers.Count, 1);
        jobHandle.Complete();

        for (int i = 0; i < healthControllers.Count; i++)
        {
            healthControllers[i].currentValue = currentHealth[i];
        }

        currentHealth.Dispose();
    }

    public int Subscribe(HealthController healthController)
    {
        ids++;
        healthControllers.Add(healthController);
        healthControllersNames.Add(healthController.gameObject.name, ids);

        idsArray.Dispose();
        idsArray = new NativeArray<int>(healthControllers.Count, Allocator.Persistent);
        for (int i = 0; i < healthControllers.Count-1; i++)
        {
            idsArray[i] = healthControllers[i].healthSystemId;
        }
        idsArray[healthControllers.Count - 1] = ids;

        return ids;
    }

    public void UnSubscribe(HealthController healthController)
    {
        healthControllers.Remove(healthController);
        healthControllersNames.Remove(healthController.gameObject.name);

        if (idsArray.IsCreated)
        {
            idsArray.Dispose();
            idsArray = new NativeArray<int>(healthControllers.Count, Allocator.Persistent);
            for (int i = 0; i < healthControllers.Count; i++)
            {
                idsArray[i] = healthControllers[i].healthSystemId;
            }
        }
    }

    // Deals damage without using the health event system
    public void TakeDamageWithoutEvent(GameObject target, float damage, int damageType)
    {
        if (LayerMask.GetMask(LayerMask.LayerToName(target.gameObject.layer)) == BasicLayerMasks.DamageableEntities)
            target.GetComponent<HealthController>().Damage(damage, damageType);
    }
    
    // Deals damage ignoring invunarable
    public event Action<string, float, int> onDamageIgnoreInvunarableTaken;
    public void TakeDamageIgnoreShields(string name, float damage, int damageType)
    {
        if (onDamageIgnoreInvunarableTaken != null)
        {
            onDamageIgnoreInvunarableTaken(name, damage, damageType);
        }
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
}
