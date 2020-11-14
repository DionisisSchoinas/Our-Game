using UnityEngine;

public class ResourceManager
{
    public class Default
    {
        public static ParticleSystem Fire = ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Lightning = ((GameObject)Resources.Load("Spells/Default Lightning Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Smoke = ((GameObject)Resources.Load("Spells/Default Smoke Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Earth = ((GameObject)Resources.Load("Spells/Default Earth Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}
