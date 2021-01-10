using UnityEngine;

public class PlayerResourcesController : MonoBehaviour
{
    public ResourceBar healthBar;
    public float maxHealth;
    public float healthRegenPerSecond;
    public bool respawn;
    public bool invulnerable;

    public ResourceBar manaBar;
    public float maxMana;
    public float manaRegenPerSecond;

    private HealthController healthController;
    private ManaController manaController;

    private void Awake()
    {
        if (healthBar != null)
        {
            healthController = gameObject.AddComponent<HealthController>();
            healthController.SetValues(maxHealth, healthRegenPerSecond, healthBar, respawn, invulnerable);
        }

        if (manaBar != null)
        {
            manaController = gameObject.AddComponent<ManaController>();
            manaController.SetValues(maxMana, manaRegenPerSecond, manaBar);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
