using Godot;

namespace Blueberry.Noise {
    [GlobalClass]
    public partial class SimpleNoiseFilter3D : NoiseFilter {
        private NoiseSettings3D NoiseSettings;

        public SimpleNoiseFilter3D(NoiseSettings3D noiseSettings, bool randomSeed = true, int seed = 0) : base(randomSeed, seed) {
            NoiseSettings = noiseSettings;
        }

        public float GetNoise(Vector3 point)
            => GetNoise(point, NoiseSettings.Center, NoiseSettings, Noise.GetNoise3Dv, Offset);

        public float GetNoise(float x, float y, float z)
            => GetNoise(new Vector3(x, y, z), NoiseSettings.Center, NoiseSettings, Noise.GetNoise3Dv, Offset);

        private Vector3 Offset(Vector3 point, float frequency, Vector3 offset)
            => point * frequency + offset;
    }
}
