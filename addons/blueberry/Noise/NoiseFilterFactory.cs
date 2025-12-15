using System;

namespace Blueberry.Noise {
    /// <summary>
    /// Factory for creating noise filter instances based on configuration settings.
    /// </summary>
    public static class NoiseFilterFactory {
        /// <summary>
        /// Creates 1D a noise filter based on the specified settings.
        /// </summary>
        /// <param name="noiseSettings">The settings that determine the filter type and parameters.</param>
        /// <returns>A new instance of a 1D noise filter.</returns>
        /// <exception cref="ArgumentException">Thrown when an unsupported filter type is specified.</exception>
        public static NoiseFilter1D CreateNoiseFilter1D(NoiseSettings1D noiseSettings) =>
            noiseSettings.Type switch {
                NoiseSettings.FilterType.Simple => new SimpleNoiseFilter1D(noiseSettings),
                NoiseSettings.FilterType.Rigid => new RigidNoiseFilter1D(noiseSettings),
                _ => throw new ArgumentException($"Unsupported filter type: {noiseSettings.Type}", nameof(noiseSettings))
            };

        /// <summary>
        /// Creates a 2D noise filter based on the specified settings.
        /// </summary>
        /// <param name="noiseSettings">The settings that determine the filter type and parameters.</param>
        /// <returns>A new instance of a 2D noise filter.</returns>
        /// <exception cref="ArgumentException">Thrown when an unsupported filter type is specified.</exception>
        public static NoiseFilter2D CreateNoiseFilter2D(NoiseSettings2D noiseSettings) =>
            noiseSettings.Type switch {
                NoiseSettings.FilterType.Simple => new SimpleNoiseFilter2D(noiseSettings),
                NoiseSettings.FilterType.Rigid => new RigidNoiseFilter2D(noiseSettings),
                _ => throw new ArgumentException($"Unsupported filter type: {noiseSettings.Type}", nameof(noiseSettings))
            };

        /// <summary>
        /// Creates a 3D noise filter based on the specified settings.
        /// </summary>
        /// <param name="noiseSettings">The settings that determine the filter type and parameters.</param>
        /// <returns>A new instance of a 3D noise filter.</returns>
        /// <exception cref="ArgumentException">Thrown when an unsupported filter type is specified.</exception>
        public static NoiseFilter3D CreateNoiseFilter3D(NoiseSettings3D noiseSettings) =>
            noiseSettings.Type switch {
                NoiseSettings.FilterType.Simple => new SimpleNoiseFilter3D(noiseSettings),
                NoiseSettings.FilterType.Rigid => new RigidNoiseFilter3D(noiseSettings),
                _ => throw new ArgumentException($"Unsupported filter type: {noiseSettings.Type}", nameof(noiseSettings))
            };
    }
}
