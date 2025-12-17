using Godot;
using System.Collections.Generic;

namespace Blueberry {
    /// <summary>
    /// A node-based state machine that manages state transitions and lifecycle.
    /// Child nodes must be State instances. The first child becomes the initial state.
    /// </summary>
    [GlobalClass]
    public partial class StateMachine : Node {
        /// <summary>
        /// Emitted when the state machine switches from one state to another.
        /// </summary>
        /// <param name="previousState">The state that was just exited.</param>
        /// <param name="nextState">The state that is now active.</param>
        [Signal]
        public delegate void StateChangedEventHandler(State previousState, State nextState);

        /// <summary>
        /// When true, the state machine will not call Entry/Exit or forward Process/PhysicsProcess/Input to states.
        /// Useful for temporarily suspending state execution without losing state information.
        /// </summary>
        [Export] public bool IsPaused { get => _isPaused; set => SetIsPaused(value); }

        /// <summary>
        /// The state that was active before the current state. Null if no state transition has occurred.
        /// </summary>
        public State PreviousState { get; private set; }

        /// <summary>
        /// The currently active state. Receives Process and PhysicsProcess calls.
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// List of all states managed by this state machine. Populated from child nodes during _Ready.
        /// </summary>
        public readonly List<State> States = new List<State>();

        private bool _isPaused;
        private bool _isInitialized;

        public override void _Ready() {
            States.Capacity = GetChildCount();
            foreach (Node child in GetChildren()) {
                if (child is State state) {
                    States.Add(state);
                }
                else {
                    string errorString = $"{child.Name} is not a state";
                    GD.PushError(errorString);
                }
            }

            if (States.Count == 0) {
                GD.PushWarning("No states found. CurrentState will be null");
                return;
            }

            CurrentState = States[0];
            if (!IsPaused) {
                UpdateState();
            }
        }

        public override void _ExitTree() {
            if (IsPaused) {
                return;
            }

            CurrentState?.Exit(null);
        }

        /// <summary>
        /// Forwards input calls to the current state.
        /// </summary>
        public override void _Input(InputEvent @event) {
            if (IsPaused) {
                return;
            }

            CurrentState?.Input(@event);
        }

        /// <summary>
        /// Forwards process calls to the current state.
        /// </summary>
        public override void _Process(double delta) {
            if (IsPaused) {
                return;
            }

            CurrentState?.Process(delta);
        }

        /// <summary>
        /// Forwards physics process calls to the current state.
        /// </summary>
        public override void _PhysicsProcess(double delta) {
            if (IsPaused) {
                return;
            }

            CurrentState?.PhysicsProcess(delta);
        }

        /// <summary>
        /// Toggles the paused state of the state machine.
        /// </summary>
        /// <returns>The new paused state (true if now paused, false if now unpaused).</returns>
        public bool TogglePause() {
            IsPaused = !IsPaused;
            return IsPaused;
        }

        /// <summary>
        /// Checks if the state machine is currently in the specified state.
        /// </summary>
        /// <param name="state">The state to check.</param>
        /// <returns>True if the current state matches the specified state.</returns>
        public bool IsInState(State state) => CurrentState == state;

        /// <summary>
        /// Checks if the state machine is currently in the state with the specified name.
        /// </summary>
        /// <param name="stateName">The name of the state to check.</param>
        /// <returns>True if the current state's name matches the specified name.</returns>
        public bool IsInState(string stateName) => CurrentState?.Name == stateName;

        /// <summary>
        /// Retrieves a state from the state machine by its node name.
        /// </summary>
        /// <param name="name">The name of the state to find.</param>
        /// <returns>The state with the specified name, or null if not found.</returns>
        public State GetState(string name) => States.Find(s => s.Name == name);

        /// <summary>
        /// Switches to the state at the specified index in the States list.
        /// </summary>
        /// <param name="newState">Index of the state to switch to.</param>
        /// <returns>True if the state was switched, false if already in that state.</returns>
        public bool SwitchState(int newState) {
            return SwitchState(States[newState]);
        }

        /// <summary>
        /// Switches to the state with the specified node name.
        /// </summary>
        /// <param name="stateName">Name of the state node to switch to.</param>
        /// <returns>True if the state was switched, false if state not found or already in that state.</returns>
        public bool SwitchState(string stateName) {
            State state = States.Find(s => s.Name == stateName);
            return state != null && SwitchState(state);
        }

        /// <summary>
        /// Switches to the specified state. Calls Exit on the current state and Entry on the new state.
        /// Emits the StateChanged signal if successful.
        /// </summary>
        /// <param name="newState">The state to switch to.</param>
        /// <returns>True if the state was switched, false if already in that state or state is not in the state machine.</returns>
        public bool SwitchState(State newState) {
            if (!States.Contains(newState)) {
                GD.PushError($"State {newState.Name} is not in the state machine");
                return false;
            }

            if (newState == CurrentState) {
                return false;
            }

            if (!IsPaused || _isInitialized) {
                PreviousState = CurrentState;
            }
            CurrentState = newState;
            _isInitialized = false;

            if (!IsPaused) {
                UpdateState();
            }

            return true;
        }

        private void SetIsPaused(bool value) {
            _isPaused = value;

            if (!IsPaused && !_isInitialized) {
                UpdateState();
            }
        }

        private void UpdateState() {
            PreviousState?.Exit(CurrentState);
            CurrentState?.Entry(PreviousState);
            _isInitialized = true;
            EmitSignal(SignalName.StateChanged, PreviousState, CurrentState);
        }
    }
}
