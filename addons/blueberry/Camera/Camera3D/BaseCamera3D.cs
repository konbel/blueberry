using Godot;

namespace Blueberry.Camera {
    /// <summary>
    /// Base camera class that monitors and signals when the camera becomes or stops being the active viewport camera.
    /// Useful for implementing camera-specific behavior that should only activate when the camera is current.
    /// When inheriting from this class, you must call base._Ready() and base._Process(delta) 
    /// in your overridden methods to ensure the CurrentChanged signal functions correctly.
    /// </summary>
    [GlobalClass]
    public partial class BaseCamera3D : Camera3D {
        /// <summary>
        /// Emitted when the camera's current status changes.
        /// </summary>
        /// <param name="current">True if the camera became the active camera, false if it lost current status.</param>
        [Signal] public delegate void CurrentChangedEventHandler(bool current);

        private bool _current;

        public override void _Ready() {
            OnCurrentChanged();
        }

        public override void _Process(double delta) {
            if (Current != _current) {
                OnCurrentChanged();
            }
        }

        private void OnCurrentChanged() {
            _current = Current;
            EmitSignal(SignalName.CurrentChanged, Current);
        }
    }
}
