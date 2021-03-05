using UnityEngine;

class BasicLayerMasks
{
    public static int Enemies = LayerMask.GetMask("Enemies");
    public static int DamageableEntities = LayerMask.GetMask("Damageables","Enemies");
    public static int IgnoreOnDamageRaycasts = LayerMask.GetMask("Damageables", "Spells");
    public static int SpellsLayers = LayerMask.GetMask("Spells");
    public static int CuttableWalls = LayerMask.GetMask("Cuttable");

    public static int EnemiesLayer = LayerMask.NameToLayer("Enemies");
    public static int DamageablesLayer = LayerMask.NameToLayer("Damageables");
}
