using Godot;

namespace Blueberry.Noise {
    /// <summary>
    /// Simple noise filter for generating 2D noise patterns.
    /// Ideal for textures, heightmaps, terrain generation, or any planar variation.
    /// </summary>
    public class SimpleNoiseFilter2D : SimpleNoiseFilter, NoiseFilter2D {
        private NoiseSettings2D NoiseSettings;

        /// <summary>
        /// Creates a new 2D noise filter with the specified settings and a random seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 2D noise generation.</param>
        public SimpleNoiseFilter2D(NoiseSettings2D noiseSettings) : base() {
            NoiseSettings = noiseSettings;
        }

        /// <summary>
        /// Creates a new 2D noise filter with the specified settings and seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 2D noise generation.</param>
        /// <param name="seed">Specific seed value for reproducible noise generation.</param>
        public SimpleNoiseFilter2D(NoiseSettings2D noiseSettings, int seed) : base(seed) {
            NoiseSettings = noiseSettings;
        }

        public float GetNoise(Vector2 point)
            => GetNoise(point, NoiseSettings.Center, NoiseSettings, Noise.GetNoise2Dv, Offset);

        public float GetNoise(float x, float y)
            => GetNoise(new Vector2(x, y), NoiseSettings.Center, NoiseSettings, Noise.GetNoise2Dv, Offset);

        private Vector2 Offset(Vector2 point, float frequency, Vector2 offset)
            => point * frequency + offset;
    }
}
