using Godot;

namespace Blueberry.Noise {
    /// <summary>
    /// Base class for noise filters that generate procedural noise values.
    /// Provides layered noise generation with configurable frequency, amplitude, and persistence.
    /// </summary>
    public class NoiseFilter {
        protected FastNoiseLite Noise;

        /// <summary>
        /// Initializes a new noise filter with a random seed.
        /// </summary>
        protected NoiseFilter() {
            RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
            int seed = randomNumberGenerator.RandiRange(int.MinValue, int.MaxValue);
            InitializeNoise(seed);
        }

        /// <summary>
        /// Initializes a new noise filter with a specific seed.
        /// </summary>
        /// <param name="seed">The seed value to use for noise generation.</param>
        protected NoiseFilter(int seed) {
            InitializeNoise(seed);
        }

        private void InitializeNoise(int seed) {
            Noise = new FastNoiseLite();
            Noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
            Noise.FractalType = FastNoiseLite.FractalTypeEnum.None;
            Noise.Frequency = 1;
            Noise.Seed = seed;
        }
    }
}
