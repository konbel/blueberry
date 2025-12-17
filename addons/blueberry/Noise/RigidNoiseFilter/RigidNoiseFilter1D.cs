namespace Blueberry.Noise {
    /// <summary>
    /// Rigid noise filter for generating 1D noise patterns.
    /// Useful for creating procedural curves, audio synthesis, or any 1-dimensional variation.
    /// </summary>
    public class RigidNoiseFilter1D : RigidNoiseFilter, NoiseFilter1D {
        private NoiseSettings1D _noiseSettings;

        /// <summary>
        /// Creates a new 1D noise filter with the specified settings and a random seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 1D noise generation.</param>
        public RigidNoiseFilter1D(NoiseSettings1D noiseSettings) : base() {
            _noiseSettings = noiseSettings;
        }

        /// <summary>
        /// Creates a new 1D noise filter with the specified settings and seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 1D noise generation.</param>
        /// <param name="seed">Specific seed value for reproducible noise generation.</param>
        public RigidNoiseFilter1D(NoiseSettings1D noiseSettings, int seed) : base(seed) {
            _noiseSettings = noiseSettings;
        }

        public float GetNoise(float point)
            => GetNoise(point, _noiseSettings.Offset, _noiseSettings, Noise.GetNoise1D, Offset);

        private float Offset(float point, float frequency, float offset)
            => point * frequency + offset;
    }
}
