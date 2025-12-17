using Godot;

namespace Blueberry.Noise {
    /// <summary>
    /// Noise settings specialized for 1D noise generation.
    /// Extends base noise settings with a 1D offset parameter.
    /// </summary>
    [GlobalClass, Tool]
    public partial class NoiseSettings1D : NoiseSettings {
        /// <summary>
        /// Offset applied to the 1D sampling position.
        /// </summary>
        [Export] public float Offset { get; set; }
    }
}
