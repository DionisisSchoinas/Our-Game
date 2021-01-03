using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public float cooldown = 0.5f;

    public abstract string Type { get; }

    public abstract string Name { get; }
}
