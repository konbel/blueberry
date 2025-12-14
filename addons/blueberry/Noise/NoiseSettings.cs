using Godot;

[GlobalClass, Tool]
public partial class NoiseSettings : Resource {
    [Export] public float Strength = 1;
    [Export(PropertyHint.Range, "1,8")] public int NumLayers = 1;
    [Export] public float BaseRoughness = 1;
    [Export] public float Roughness = 2;
    [Export] public float Persistence = 0.5f;
    [Export] public float MinValue = 0;
}
