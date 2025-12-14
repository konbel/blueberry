using Godot;

namespace Blueberry.Noise {
    /// <summary>
    /// Simple noise filter for generating 3D noise patterns.
    /// Perfect for volumetric effects, 3D terrain, procedural shapes, or spatial variation.
    /// </summary>
    public partial class SimpleNoiseFilter3D : NoiseFilter {
        private NoiseSettings3D NoiseSettings;

        /// <summary>
        /// Creates a new 3D noise filter with the specified settings and a random seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 3D noise generation.</param>
        public SimpleNoiseFilter3D(NoiseSettings3D noiseSettings) : base() {
            NoiseSettings = noiseSettings;
        }

        /// <summary>
        /// Creates a new 3D noise filter with the specified settings and seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 3D noise generation.</param>
        /// <param name="seed">Specific seed value for reproducible noise generation.</param>
        public SimpleNoiseFilter3D(NoiseSettings3D noiseSettings, int seed) : base(seed) {
            NoiseSettings = noiseSettings;
        }

        /// <summary>
        /// Samples noise at the specified 3D point.
        /// </summary>
        /// <param name="point">The 3D position to sample.</param>
        /// <returns>The noise value at the given point.</returns>
        public float GetNoise(Vector3 point)
            => GetNoise(point, NoiseSettings.Center, NoiseSettings, Noise.GetNoise3Dv, Offset);

        /// <summary>
        /// Samples noise at the specified 3D coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>The noise value at the given coordinates.</returns>
        public float GetNoise(float x, float y, float z)
            => GetNoise(new Vector3(x, y, z), NoiseSettings.Center, NoiseSettings, Noise.GetNoise3Dv, Offset);

        private Vector3 Offset(Vector3 point, float frequency, Vector3 offset)
            => point * frequency + offset;
    }
}
