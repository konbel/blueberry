using Godot;

/// <summary>
/// Base configuration settings for procedural noise generation.
/// Controls octave layering, frequency, amplitude, and overall strength of the noise pattern.
/// </summary>
[GlobalClass, Tool]
public partial class NoiseSettings : Resource {
    /// <summary>
    /// Multiplier for the final noise output. Higher values produce more pronounced effects.
    /// </summary>
    [Export] public float Strength = 1;

    /// <summary>
    /// Number of noise octaves (layers) to combine. More layers add finer detail.
    /// </summary>
    [Export(PropertyHint.Range, "1,8")] public int NumLayers = 1;

    /// <summary>
    /// Initial frequency scale for the first octave. Affects the base size of noise features.
    /// </summary>
    [Export] public float BaseRoughness = 1;

    /// <summary>
    /// Frequency multiplier applied to each successive octave. Higher values add finer details.
    /// </summary>
    [Export] public float Roughness = 2;

    /// <summary>
    /// Amplitude multiplier for each successive octave. Controls how much each layer contributes.
    /// </summary>
    [Export] public float Persistence = 0.5f;

    /// <summary>
    /// Minimum threshold value. Noise values below this are clamped to zero.
    /// </summary>
    [Export] public float MinValue = 0;
}
