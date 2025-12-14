using Godot;
using System;

namespace Blueberry.Noise {
    public partial class NoiseFilter : Node {
        protected FastNoiseLite Noise;

        protected NoiseFilter(bool randomSeed = true, int seed = 0) {
            if (randomSeed) {
                RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
                seed = randomNumberGenerator.RandiRange(int.MinValue, int.MaxValue);
            }

            Noise = new FastNoiseLite();
            Noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
            Noise.FractalType = FastNoiseLite.FractalTypeEnum.None;
            Noise.Frequency = 1;
            Noise.Seed = seed;
        }

        protected float GetNoise<T>(T point, T offset, NoiseSettings noiseSettings, Func<T, float> noiseFunction, Func<T, float, T, T> offsetFunction) {
            float noiseValue = 0;
            float frequency = noiseSettings.BaseRoughness;
            float amplitude = 1;

            for (int i = 0; i < noiseSettings.NumLayers; i++) {
                float v = noiseFunction(offsetFunction(point, frequency, offset));
                noiseValue += (v + 1) * 0.5f * amplitude;
                frequency *= noiseSettings.Roughness;
                amplitude *= noiseSettings.Persistence;
            }

            noiseValue = Mathf.Max(0, noiseValue - noiseSettings.MinValue);
            return noiseValue * noiseSettings.Strength;
        }
    }
}
