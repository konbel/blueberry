using Godot;
using static Blueberry.Utils;

namespace Blueberry.Camera {
    /// <summary>
    /// A free-flying camera controller supporting both mouse and gamepad input.
    /// Provides unrestricted 3D movement and rotation, useful for development, debugging, or spectator modes.
    /// </summary>
    [GlobalClass]
    public partial class FreeCamera3D : BaseCamera3D {
        #region Fields

        [ExportGroup("Movement Settings")]
        /// <summary>
        /// Input action for moving forward in the camera's local -Z direction.
        /// </summary>
        [Export] public StringName Forward { get; set; }

        /// <summary>
        /// Input action for moving backward in the camera's local +Z direction.
        /// </summary>
        [Export] public StringName Backward { get; set; }

        /// <summary>
        /// Input action for moving left in the camera's local -X direction.
        /// </summary>
        [Export] public StringName Left { get; set; }

        /// <summary>
        /// Input action for moving right in the camera's local +X direction.
        /// </summary>
        [Export] public StringName Right { get; set; }

        /// <summary>
        /// Input action for moving up in the camera's local +Y direction.
        /// </summary>
        [Export] public StringName Up { get; set; }

        /// <summary>
        /// Input action for moving down in the camera's local -Y direction.
        /// </summary>
        [Export] public StringName Down { get; set; }

        /// <summary>
        /// Movement speed in units per second.
        /// </summary>
        [Export] public float MoveSpeed { get; set; }

        [ExportGroup("Look Settings")]
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

        /// <summary>
        /// Maximum pitch angle in degrees. Prevents camera from flipping over.
        /// </summary>
        [Export] public float UpperCap { get; set; } = 90f;

        /// <summary>
        /// Minimum pitch angle in degrees. Prevents camera from flipping over.
        /// </summary>
        [Export] public float LowerCap { get; set; } = -90f;

        private float _yaw;
        private float _pitch;

        #endregion

        public override void _Ready() {
            ValidateInputAction("FreeCamera3D", Forward, nameof(Forward));
            ValidateInputAction("FreeCamera3D", Backward, nameof(Backward));
            ValidateInputAction("FreeCamera3D", Left, nameof(Left));
            ValidateInputAction("FreeCamera3D", Right, nameof(Right));
            ValidateInputAction("FreeCamera3D", Up, nameof(Up));
            ValidateInputAction("FreeCamera3D", Down, nameof(Down));
            ValidateInputAction("FreeCamera3D", LookRight, nameof(LookRight));
            ValidateInputAction("FreeCamera3D", LookLeft, nameof(LookLeft));
            ValidateInputAction("FreeCamera3D", LookUp, nameof(LookUp));
            ValidateInputAction("FreeCamera3D", LookDown, nameof(LookDown));

            _yaw = Rotation.Y;
            _pitch = Rotation.X;

            base._Ready();
        }

        public override void _Input(InputEvent @event) {
            if (@event is InputEventMouseMotion mouseMotion) {
                Look(mouseMotion.Relative.X, mouseMotion.Relative.Y, MouseSensitivity);
            }
        }

        public override void _PhysicsProcess(double delta) {
            float lookX = Input.GetAxis(LookLeft, LookRight);
            float lookY = Input.GetAxis(LookUp, LookDown);

            if (lookX != 0 || lookY != 0) {
                Look(lookX, lookY, ControllerSensitivity * (float)delta);
            }

            Move(delta);
        }

        /// <summary>
        /// Updates camera rotation based on look input.
        /// Handles both mouse and controller input with independent yaw and pitch tracking.
        /// </summary>
        /// <param name="x">Horizontal look input</param>
        /// <param name="y">Vertical look input</param>
        /// <param name="sensitivity">Sensitivity multiplier for this input type</param>
        private void Look(float x, float y, float sensitivity) {
            _yaw -= sensitivity * x;
            _pitch -= sensitivity * y;

            _pitch = Mathf.Clamp(_pitch, Mathf.DegToRad(LowerCap), Mathf.DegToRad(UpperCap));

            Rotation = new Vector3(_pitch, _yaw, 0);
        }

        /// <summary>
        /// Handles camera movement based on input actions.
        /// Movement is relative to the camera's local transform (right, up, forward).
        /// </summary>
        /// <param name="delta">Time elapsed since last frame in seconds</param>
        private void Move(double delta) {
            float forward = Input.GetAxis(Forward, Backward);
            float right = Input.GetAxis(Left, Right);
            float up = Input.GetAxis(Down, Up);

            Basis basis = Transform.Basis;
            Vector3 direction = Vector3.Zero;

            direction += basis.X * right;
            direction += basis.Y * up;
            direction += basis.Z * forward;

            Position += direction.Normalized() * MoveSpeed * (float)delta;
        }
    }
}
