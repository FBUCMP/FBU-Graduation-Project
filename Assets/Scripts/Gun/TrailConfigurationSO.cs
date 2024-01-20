using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Guns/Gun Trail Configuration", order = 4)]
public class TrailConfigurationSO : ScriptableObject, System.ICloneable
{
    // for trail renderer 
    public Material material;
    public AnimationCurve widthCurve;
    public float duration = 0.5f;
    public float minVertexDistance = 0.1f;
    public Gradient color;

    public float missDistance = 100f;
    public float simulationSpeed = 100f;



    // ICloneable interface and Clone() for using copies of scriptable objects instead
    public object Clone()
    {
        TrailConfigurationSO config = CreateInstance<TrailConfigurationSO>();
        Utilities.CopyValues(this, config); // Utilities is a custom class
        return config;
    }
}
