using UnityEngine;

public class ResourceManager
{
    public class Default
    {
        private static string folder = "Spells/";
        public static ParticleSystem Fire = ((GameObject)Resources.Load(folder + "Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Lightning = ((GameObject)Resources.Load(folder + "Default Lightning Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Smoke = ((GameObject)Resources.Load(folder + "Default Smoke Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Earth = ((GameObject)Resources.Load(folder + "Default Earth Source", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Ice = ((GameObject)Resources.Load(folder + "Default Ice Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }

    public class Effects
    {
        private static string folder = "Effects/";
        public static ParticleSystem Burning = ((GameObject)Resources.Load(folder + "Burning Effect", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Electrified = ((GameObject)Resources.Load(folder + "Electrified Effect", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Frozen = ((GameObject)Resources.Load(folder + "Frozen Effect", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Hit = ((GameObject)Resources.Load(folder + "Hit Effect", typeof(GameObject))).GetComponent<ParticleSystem>();
        public static ParticleSystem Sparks = ((GameObject)Resources.Load(folder + "Sparks", typeof(GameObject))).GetComponent<ParticleSystem>();

        public class Sword
        {
            private static string folder = Effects.folder + "Sword/";
            public class Trails
            {
                private static string folder = Sword.folder + "Trails/";
                public static SwingTrailRenderer Default = ((GameObject)Resources.Load(folder + "Sword Default Trail", typeof(GameObject))).GetComponent<SwingTrailRenderer>();
                public static SwingTrailRenderer Fire = ((GameObject)Resources.Load(folder + "Sword Fire Trail", typeof(GameObject))).GetComponent<SwingTrailRenderer>();
                public static SwingTrailRenderer Ice = ((GameObject)Resources.Load(folder + "Sword Ice Trail", typeof(GameObject))).GetComponent<SwingTrailRenderer>();
                public static SwingTrailRenderer Lightning = ((GameObject)Resources.Load(folder + "Sword Lightning Trail", typeof(GameObject))).GetComponent<SwingTrailRenderer>();
                public static SwingTrailRenderer Earth = ((GameObject)Resources.Load(folder + "Sword Earth Trail", typeof(GameObject))).GetComponent<SwingTrailRenderer>();
            }
        }
    }

    public class Components
    {
        private static string folder = "Components/"; 
        public static Arc Arc = ((GameObject)Resources.Load(folder + "Arc", typeof(GameObject))).GetComponent<Arc>();
        public static GameObject IndicatorBase = (GameObject)Resources.Load(folder + "Quad Base Indicator", typeof(GameObject));
    }

    public class Materials
    {
        private static string folder = "Materials/";
        public static Material LightningArc1 = (Material)Resources.Load(folder + "Lightning Arc 1", typeof(Material));
        public static Material LightningArc2 = (Material)Resources.Load(folder + "Lightning Arc 2", typeof(Material));
        public static Material IndicatorCircleAOE = (Material)Resources.Load(folder + "AOE Circle Indicator Material", typeof(Material));
        public static Material IndicatorSquareAOE = (Material)Resources.Load(folder + "AOE Square Indicator Material", typeof(Material));
        public static Material IndicatorTriangleAOE = (Material)Resources.Load(folder + "AOE Triangle Indicator Material", typeof(Material));
        public static Material IndicatorCirlceRange = (Material)Resources.Load(folder + "Range Circle Indicator Material", typeof(Material));

        public class Resistances
        {
            private static string folder = Materials.folder + "Resistances/";
            public static Material Fire = (Material)Resources.Load(folder + "Fire Resistance", typeof(Material));
            public static Material Ice = (Material)Resources.Load(folder + "Ice Resistance", typeof(Material));
            public static Material Lightning = (Material)Resources.Load(folder + "Lightning Resistance", typeof(Material));
            public static Material Physical = (Material)Resources.Load(folder + "Physical Resistance", typeof(Material));
        }
    }
}
