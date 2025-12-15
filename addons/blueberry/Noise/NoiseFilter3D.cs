using Godot;

namespace Blueberry.Noise {
    public interface NoiseFilter3D {
        /// <summary>
        /// Samples noise at the specified 3D point.
        /// </summary>
        /// <param name="point">The 3D position to sample.</param>
        /// <returns>The noise value at the given point.</returns>
        public float GetNoise(Vector3 point);

        /// <summary>
        /// Samples noise at the specified 3D coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>The noise value at the given coordinates.</returns>
        public float GetNoise(float x, float y, float z);
    }
}
