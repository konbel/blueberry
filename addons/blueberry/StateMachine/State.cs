using Godot;

namespace Blueberry {
    /// <summary>
    /// Base class for state machine states. Inherit from this class to create custom states.
    /// Must be a child of a StateMachine node to function properly.
    /// </summary>
    [GlobalClass]
    public partial class State : Node {
        /// <summary>
        /// Called when entering this state. Override to implement entry behavior.
        /// </summary>
        /// <param name="previousState">The state that was active before this one. Null if this is the first state.</param>
        public virtual void Entry(State previousState) {}
        
        /// <summary>
        /// Called when an input event occurs while this state is active. Override to implement input handling.
        /// </summary>
        /// <param name="event">The input event that occurred.</param>
        public virtual void Input(InputEvent @event) {}
        
        /// <summary>
        /// Called every frame while this state is active. Override to implement per-frame logic.
        /// </summary>
        /// <param name="delta">Time elapsed since the last frame in seconds.</param>
        public virtual void Process(double delta) {}
        
        /// <summary>
        /// Called every physics frame while this state is active. Override to implement physics logic.
        /// </summary>
        /// <param name="delta">Time elapsed since the last physics frame in seconds.</param>
        public virtual void PhysicsProcess(double delta) {}
        
        /// <summary>
        /// Called when leaving this state. Override to implement exit/cleanup behavior.
        /// </summary>
        /// <param name="nextState">The state that will become active after this one.</param>
        public virtual void Exit(State nextState) {}
    }
}
