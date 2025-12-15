using Godot;
using System;

namespace Blueberry.Noise {
    public class RigidNoiseFilter : NoiseFilter {
        /// <summary>
        /// Initializes a new noise filter with a random seed.
        /// </summary>
        protected RigidNoiseFilter() : base() { }

        /// <summary>
        /// Initializes a new noise filter with a specific seed.
        /// </summary>
        /// <param name="seed">The seed value to use for noise generation.</param>
        protected RigidNoiseFilter(int seed) : base(seed) { }

        /// <summary>
        /// Generates layered noise at the specified point using the provided noise settings.
        /// Applies multiple octaves with varying frequency and amplitude for detailed noise patterns.
        /// </summary>
        /// <typeparam name="T">The type of point (float, Vector2, or Vector3).</typeparam>
        /// <param name="point">The position at which to sample noise.</param>
        /// <param name="offset">The offset to apply to the sampling position.</param>
        /// <param name="noiseSettings">Settings that control noise generation parameters.</param>
        /// <param name="noiseFunction">Function to sample noise at a given point.</param>
        /// <param name="offsetFunction">Function to apply frequency and offset transformations.</param>
        /// <returns>The computed noise value, scaled by strength and clamped by minimum value.</returns>
        protected float GetNoise<T>(T point, T offset, NoiseSettings noiseSettings, Func<T, float> noiseFunction, Func<T, float, T, T> offsetFunction) {
            float noiseValue = 0;
            float frequency = noiseSettings.BaseRoughness;
            float amplitude = 1;
            float weight = 1;

            for (int i = 0; i < noiseSettings.NumLayers; i++) {
                float v = 1 - Mathf.Abs(noiseFunction(offsetFunction(point, frequency, offset)));
                v *= v;
                v *= weight;
                weight = Mathf.Clamp(v * noiseSettings.WeightMultiplier, 0, 1);

                noiseValue += v * amplitude;
                frequency *= noiseSettings.Roughness;
                amplitude *= noiseSettings.Persistence;
            }

            noiseValue = Mathf.Max(0, noiseValue - noiseSettings.MinValue);
            return noiseValue * noiseSettings.Strength;
        }
    }
}
