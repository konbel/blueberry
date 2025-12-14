using Godot;

namespace Blueberry.Noise {
    [GlobalClass]
    public partial class SimpleNoiseFilter2D : NoiseFilter {
        private NoiseSettings2D NoiseSettings;

        public SimpleNoiseFilter2D(NoiseSettings2D noiseSettings, bool randomSeed = true, int seed = 0) : base(randomSeed, seed) {
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
