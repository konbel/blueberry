using Godot;

namespace Blueberry.Noise {
    public interface NoiseFilter2D {
        /// <summary>
        /// Samples noise at the specified 2D point.
        /// </summary>
        /// <param name="point">The 2D position to sample.</param>
        /// <returns>The noise value at the given point.</returns>
        public float GetNoise(Vector2 point);

        /// <summary>
        /// Samples noise at the specified 2D coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The noise value at the given coordinates.</returns>
        public float GetNoise(float x, float y);
    }
}
