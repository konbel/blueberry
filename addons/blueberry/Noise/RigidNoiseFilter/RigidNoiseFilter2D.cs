using Godot;

namespace Blueberry.Noise {
    /// <summary>
    /// Rigid noise filter for generating 2D noise patterns.
    /// Ideal for textures, heightmaps, terrain generation, or any planar variation.
    /// </summary>
    public class RigidNoiseFilter2D : RigidNoiseFilter, NoiseFilter2D {
        private NoiseSettings2D _noiseSettings;

        /// <summary>
        /// Creates a new 2D noise filter with the specified settings and a random seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 2D noise generation.</param>
        public RigidNoiseFilter2D(NoiseSettings2D noiseSettings) : base() {
            _noiseSettings = noiseSettings;
        }

        /// <summary>
        /// Creates a new 2D noise filter with the specified settings and seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 2D noise generation.</param>
        /// <param name="seed">Specific seed value for reproducible noise generation.</param>
        public RigidNoiseFilter2D(NoiseSettings2D noiseSettings, int seed) : base(seed) {
            _noiseSettings = noiseSettings;
        }

        public float GetNoise(Vector2 point)
            => GetNoise(point, _noiseSettings.Center, _noiseSettings, Noise.GetNoise2Dv, Offset);

        public float GetNoise(float x, float y)
            => GetNoise(new Vector2(x, y), _noiseSettings.Center, _noiseSettings, Noise.GetNoise2Dv, Offset);

        private Vector2 Offset(Vector2 point, float frequency, Vector2 offset)
            => point * frequency + offset;
    }
}
