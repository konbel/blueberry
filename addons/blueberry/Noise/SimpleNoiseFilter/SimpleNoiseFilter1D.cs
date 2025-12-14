using Godot;

namespace Blueberry.Noise {
    [GlobalClass]
    public partial class SimpleNoiseFilter1D : NoiseFilter {
        private NoiseSettings1D NoiseSettings;

        public SimpleNoiseFilter1D(NoiseSettings1D noiseSettings, bool randomSeed = true, int seed = 0) : base(randomSeed, seed) {
            NoiseSettings = noiseSettings;
        }

        public float GetNoise(float point)
            => GetNoise(point, NoiseSettings.Offset, NoiseSettings, Noise.GetNoise1D, Offset);

        private float Offset(float point, float frequency, float offset)
            => point * frequency + offset;
    }
}
