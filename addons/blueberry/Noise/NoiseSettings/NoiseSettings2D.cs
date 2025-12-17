using Godot;

namespace Blueberry.Noise {
    /// <summary>
    /// Noise settings specialized for 2D noise generation.
    /// Extends base noise settings with a 2D center offset parameter.
    /// </summary>
    [GlobalClass, Tool]
    public partial class NoiseSettings2D : NoiseSettings {
        /// <summary>
        /// Center offset applied to 2D sampling positions.
        /// </summary>
        [Export] public Vector2 Center { get; set; }
    }
}
