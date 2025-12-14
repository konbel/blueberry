using Godot;

/// <summary>
/// Noise settings specialized for 3D noise generation.
/// Extends base noise settings with a 3D center offset parameter.
/// </summary>
[GlobalClass, Tool]
public partial class NoiseSettings3D : NoiseSettings {
    /// <summary>
    /// Center offset applied to 3D sampling positions.
    /// </summary>
    [Export] public Vector3 Center;
}
