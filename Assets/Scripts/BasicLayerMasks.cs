using UnityEngine;

class BasicLayerMasks
{
    public static int DamageableEntities = LayerMask.GetMask("Damageables");
    public static int IgnoreOnDamageRaycasts = LayerMask.GetMask("Damageables", "Spells");
}
