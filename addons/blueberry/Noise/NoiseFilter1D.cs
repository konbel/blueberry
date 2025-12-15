namespace Blueberry.Noise {
    public interface NoiseFilter1D {
        /// <summary>
        /// Samples noise at the specified 1D point.
        /// </summary>
        /// <param name="point">The position along the 1D axis to sample.</param>
        /// <returns>The noise value at the given point.</returns>
        public float GetNoise(float point);
    }
}
