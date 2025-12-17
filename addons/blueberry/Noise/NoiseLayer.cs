using Godot;

namespace Blueberry.Noise {
    /// <summary>
    /// Represents a configurable layer of noise that can be combined with other layers.
    /// Used for creating complex noise patterns by stacking and masking multiple noise sources.
    /// </summary>
    [GlobalClass, Tool]
    public partial class NoiseLayer : Resource {
        /// <summary>
        /// Whether this noise layer is active and should be applied.
        /// </summary>
        [Export] public bool Enabled { get; set; } = true;

        /// <summary>
        /// When true, uses the first layer as a mask to modulate this layer's contribution.
        /// </summary>
        [Export] public bool UseFirstLayerAsMask { get; set; }

        /// <summary>
        /// The noise settings that define how this layer's noise is generated.
        /// </summary>
        [Export] public NoiseSettings3D NoiseSettings { get; set; }
    }
}
