using Godot;

namespace Blueberry {
    public static class Utils {
        internal static Color GizmoColor = new Color(1, 0.5f, 0);
        internal static Color TransparentGizmoColor = new Color(1, 0.5f, 0, 0.7f);

        /// <summary>
        /// Validates that an input action exists in the project's InputMap.
        /// Logs an error if the action is not found.
        /// </summary>
        /// <param name="caller">The function or script calling this method for error context</param>
        /// <param name="action">The input action name to validate</param>
        /// <param name="propertyName">Optional property name for error context</param>
        /// <returns>True if the action exists, false otherwise</returns>
        public static bool ValidateInputAction(string caller, StringName action, string propertyName = null) {
            if (!InputMap.HasAction(action)) {
                string context = propertyName != null ? $" (property: {propertyName})" : "";
                GD.PushError($"{caller}: Input action \"{action}\"{context} not found in Project Settings. Please add it or check for typos.");
                return false;
            }
            return true;
        }
    }
}