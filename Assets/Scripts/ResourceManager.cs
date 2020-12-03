using UnityEngine;

public class ResourceManager
{
    public class Default
    {
        public static ParticleSystem Fire = ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Lightning = ((GameObject)Resources.Load("Spells/Default Lightning Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Smoke = ((GameObject)Resources.Load("Spells/Default Smoke Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Earth = ((GameObject)Resources.Load("Spells/Default Earth Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Ice = ((GameObject)Resources.Load("Spells/Default Ice Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }

    public class Effects
    {
        public static ParticleSystem Burning = ((GameObject)Resources.Load("Effects/Burning Effect", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Electrified = ((GameObject)Resources.Load("Effects/Electrified Effect", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Frozen = ((GameObject)Resources.Load("Effects/Frozen Effect", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Hit = ((GameObject)Resources.Load("Effects/Hit Effect", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Sparks = ((GameObject)Resources.Load("Effects/Sparks", typeof(GameObject))).GetComponent<ParticleSystem>();
    }

    public class Components
    {
        public static Arc Arc = ((GameObject)Resources.Load("Components/Arc", typeof(GameObject))).GetComponent<Arc>();
        public static GameObject IndicatorBase = (GameObject)Resources.Load("Components/Quad Base Indicator", typeof(GameObject));
    }
    public class Materials
    {
        public static Material LightningArc1 = (Material)Resources.Load("Materials/Lightning Arc 1", typeof(Material));
        public static Material LightningArc2 = (Material)Resources.Load("Materials/Lightning Arc 2", typeof(Material));
        public static Material IndicatorCircleAOE = (Material)Resources.Load("Materials/AOE Circle Indicator Material", typeof(Material));
        public static Material IndicatorSquareAOE = (Material)Resources.Load("Materials/AOE Square Indicator Material", typeof(Material));
        public static Material IndicatorCirlceRange = (Material)Resources.Load("Materials/Range Circle Indicator Material", typeof(Material));
    }
}
