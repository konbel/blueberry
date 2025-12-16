using Godot;
using static Blueberry.Utils;

namespace Blueberry.Camera {
    /// <summary>
    /// An orbital camera that rotates around a pivot point at a fixed radius.
    /// Supports both mouse and gamepad input with optional pitch and yaw constraints.
    /// </summary>
    [GlobalClass, Tool]
    public partial class OrbitalCamera3D : BaseCamera3D {
        #region Fields

        /// <summary>
        /// Distance from the pivot point at which the camera orbits.
        /// </summary>
        [Export]
        public float Radius {
            get => _radius;
            set {
                _radius = value;
                UpdatePosition();
            }
        }
        private float _radius;

        /// <summary>
        /// The center point around which the camera orbits.
        /// </summary>
        [Export]
        public Vector3 Pivot {
            get => _pivot;
            set {
                _pivot = value;
                UpdatePosition();
            }
        }
        private Vector3 _pivot;

        [ExportGroup("Input")]
        /// <summary>
        /// Input action for looking right (negative yaw). Used for non mouse rotation.
        /// </summary>
        [Export] public StringName LookRight { get; set; }

        /// <summary>
        /// Input action for looking left (positive yaw). Used for non mouse rotation.
        /// </summary>
        [Export] public StringName LookLeft { get; set; }

        /// <summary>
        /// Input action for looking up (positive pitch). Used for non mouse rotation.
        /// </summary>
        [Export] public StringName LookUp { get; set; }

        /// <summary>
        /// Input action for looking down (negative pitch). Used for non mouse rotation.
        /// </summary>
        [Export] public StringName LookDown { get; set; }

        /// <summary>
        /// Mouse look sensitivity multiplier. Lower values result in slower camera rotation.
        /// </summary>
        [Export] public float MouseSensitivity { get; set; } = 0.002f;

        /// <summary>
        /// Controller/gamepad look sensitivity multiplier. Lower values result in slower camera rotation.
        /// </summary>
        [Export] public float ControllerSensitivity { get; set; } = 3.0f;

        [ExportGroup("Pitch")]
        /// <summary>
        /// Whether to constrain the pitch angle within MinPitch and MaxPitch bounds.
        /// </summary>
        [Export] public bool LimitPitch { get; set; } = false;

        /// <summary>
        /// Maximum pitch angle in degrees. Prevents camera from flipping over.
        /// </summary>
        [Export] public float MaxPitch { get; set; } = 90f;

        /// <summary>
        /// Minimum pitch angle in degrees. Prevents camera from flipping over.
        /// </summary>
        [Export] public float MinPitch { get; set; } = -90f;

        [ExportGroup("Yaw")]
        /// <summary>
        /// Whether to constrain the yaw angle within MinYaw and MaxYaw bounds.
        /// </summary>
        [Export] public bool LimitYaw { get; set; } = false;

        /// <summary>
        /// Maximum yaw angle in degrees.
        /// </summary>
        [Export] public float MaxYaw { get; set; } = 90f;

        /// <summary>
        /// Minimum yaw angle in degrees.
        /// </summary>
        [Export] public float MinYaw { get; set; } = -90f;

        private float _yaw = 0f;
        private float _pitch = 0f;
        private MeshInstance3D _pivotGizmo;

        #endregion

        public override void _Ready() {
            if (Engine.IsEditorHint()) {
                CreatePivotGizmo();
                return;
            }

            ValidateInputAction("OrbitalCamera3D", LookRight, nameof(LookRight));
            ValidateInputAction("OrbitalCamera3D", LookLeft, nameof(LookLeft));
            ValidateInputAction("OrbitalCamera3D", LookUp, nameof(LookUp));
            ValidateInputAction("OrbitalCamera3D", LookDown, nameof(LookDown));

            _yaw = Rotation.Y;
            _pitch = Rotation.X;

            UpdatePosition();

            base._Ready();
        }

        public override void _Input(InputEvent @event) {
            if (Engine.IsEditorHint()) {
                return;
            }

            if (@event is InputEventMouseMotion mouseMotion) {
                Look(mouseMotion.Relative.X, mouseMotion.Relative.Y, MouseSensitivity);
            }
        }

        public override void _PhysicsProcess(double delta) {
            if (Engine.IsEditorHint()) {
                return;
            }

            float lookX = Input.GetAxis(LookLeft, LookRight);
            float lookY = Input.GetAxis(LookUp, LookDown);

            if (lookX != 0 || lookY != 0) {
                Look(lookX, lookY, ControllerSensitivity * (float)delta);
            }
        }

        /// <summary>
        /// Updates camera rotation and position to orbit around the pivot point.
        /// Uses spherical coordinates to calculate the camera's position at the specified radius.
        /// </summary>
        /// <param name="x">Horizontal look input</param>
        /// <param name="y">Vertical look input</param>
        /// <param name="sensitivity">Sensitivity multiplier for this input type</param>
        private void Look(float x, float y, float sensitivity) {
            _yaw += sensitivity * x;
            _pitch -= sensitivity * y;

            if (LimitYaw) {
                _yaw = Mathf.Clamp(_yaw, Mathf.DegToRad(MinYaw), Mathf.DegToRad(MaxYaw));
            }

            if (LimitPitch) {
                _pitch = Mathf.Clamp(_pitch, Mathf.DegToRad(MinPitch), Mathf.DegToRad(MaxPitch));
            }

            UpdatePosition();
        }

        /// <summary>
        /// Updates the camera position based on current yaw, pitch, radius, and pivot.
        /// </summary>
        private void UpdatePosition() {
            // Calculate orbital position using spherical coordinates
            Vector3 offset = new Vector3(
                Mathf.Cos(_pitch) * Mathf.Sin(_yaw),
                Mathf.Sin(_pitch),
                Mathf.Cos(_pitch) * Mathf.Cos(_yaw)
            ) * _radius;

            Position = _pivot + offset;
            LookAtFromPosition(Position, _pivot, Vector3.Up);

            if (Engine.IsEditorHint() && _pivotGizmo != null) {
                _pivotGizmo.GlobalPosition = _pivot;
            }
        }

        /// <summary>
        /// Creates a visual gizmo to show the pivot point in the editor.
        /// </summary>
        private void CreatePivotGizmo() {
            // Remove existing gizmo if it exists
            if (_pivotGizmo != null && IsInstanceValid(_pivotGizmo)) {
                _pivotGizmo.QueueFree();
            }

            // Create sphere mesh for the pivot gizmo
            var sphere = new SphereMesh();
            sphere.Radius = 0.05f;
            sphere.Height = 0.1f;

            // Create material with bright color
            var material = new StandardMaterial3D();
            material.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
            material.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
            material.AlbedoColor = TransparentGizmoColor;

            // Create mesh instance
            _pivotGizmo = new MeshInstance3D();
            _pivotGizmo.Mesh = sphere;
            _pivotGizmo.MaterialOverride = material;
            _pivotGizmo.GlobalPosition = _pivot;
            _pivotGizmo.CastShadow = GeometryInstance3D.ShadowCastingSetting.Off;

            AddChild(_pivotGizmo);
        }
    }
}
