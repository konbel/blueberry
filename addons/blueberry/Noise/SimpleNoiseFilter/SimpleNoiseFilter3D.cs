using Godot;

namespace Blueberry.Noise {
    /// <summary>
    /// Simple noise filter for generating 3D noise patterns.
    /// Perfect for volumetric effects, 3D terrain, procedural shapes, or spatial variation.
    /// </summary>
    public class SimpleNoiseFilter3D : SimpleNoiseFilter, NoiseFilter3D {
        private NoiseSettings3D _noiseSettings;

        /// <summary>
        /// Creates a new 3D noise filter with the specified settings and a random seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 3D noise generation.</param>
        public SimpleNoiseFilter3D(NoiseSettings3D noiseSettings) : base() {
            _noiseSettings = noiseSettings;
        }

        /// <summary>
        /// Creates a new 3D noise filter with the specified settings and seed.
        /// </summary>
        /// <param name="noiseSettings">Configuration settings for 3D noise generation.</param>
        /// <param name="seed">Specific seed value for reproducible noise generation.</param>
        public SimpleNoiseFilter3D(NoiseSettings3D noiseSettings, int seed) : base(seed) {
            _noiseSettings = noiseSettings;
        }

        public float GetNoise(Vector3 point)
            => GetNoise(point, _noiseSettings.Center, _noiseSettings, Noise.GetNoise3Dv, Offset);

        public float GetNoise(float x, float y, float z)
            => GetNoise(new Vector3(x, y, z), _noiseSettings.Center, _noiseSettings, Noise.GetNoise3Dv, Offset);

        private Vector3 Offset(Vector3 point, float frequency, Vector3 offset)
            => point * frequency + offset;
    }
}
