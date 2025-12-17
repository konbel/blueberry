using Godot;
using System;
using Blueberry;
using System.Collections.Generic;

public partial class StateMachineExample : Node {
    private StateMachine _stateMachine;
    private List<string> _log = new List<string>();

    public override void _Ready() {
        // Run all test cases
        TestInitialStateEntry();
        TestBasicStateSwitching();
        TestStartPausedSwitchUnpause();
        TestMultipleSwitchesWhilePaused();
        TestSwitchPauseSwitchUnpause();
        TestPauseUnpauseWithoutSwitch();

        GD.Print("All state machine tests completed!");
    }

    private void TestInitialStateEntry() {
        GD.Print("\n=== Test: Initial State Entry ===");
        _log.Clear();

        _stateMachine = new StateMachine();

        var state1 = new TestState("State1", _log);
        var state2 = new TestState("State2", _log);
        _stateMachine.AddChild(state1);
        _stateMachine.AddChild(state2);

        AddChild(_stateMachine);

        AssertLog(new[] { "State1.Entry(null)" }, "Initial state should have Entry called");

        _stateMachine.QueueFree();
    }

    private void TestBasicStateSwitching() {
        GD.Print("\n=== Test: Basic State Switching ===");
        _log.Clear();

        _stateMachine = new StateMachine();

        var state1 = new TestState("State1", _log);
        var state2 = new TestState("State2", _log);
        _stateMachine.AddChild(state1);
        _stateMachine.AddChild(state2);

        AddChild(_stateMachine);

        _stateMachine.SwitchState(1);

        AssertLog(new[] {
            "State1.Entry(null)",
            "State1.Exit(State2)",
            "State2.Entry(State1)"
        }, "State switch should call Exit then Entry");

        _stateMachine.QueueFree();
    }

    private void TestStartPausedSwitchUnpause() {
        GD.Print("\n=== Test: Start Paused, Switch While Paused, Unpause ===");
        _log.Clear();

        _stateMachine = new StateMachine();
        _stateMachine.IsPaused = true;

        var state1 = new TestState("State1", _log);
        var state2 = new TestState("State2", _log);
        _stateMachine.AddChild(state1);
        _stateMachine.AddChild(state2);

        AddChild(_stateMachine);

        AssertLog(new string[] { }, "No Entry should be called when starting paused");

        _log.Clear();
        _stateMachine.SwitchState(1);

        AssertLog(new string[] { }, "No Entry/Exit while paused");

        _log.Clear();
        _stateMachine.IsPaused = false;

        AssertLog(new[] {
            "State2.Entry(null)"
        }, "On unpause, only Entry on current state (State1 never entered, so no Exit)");

        _stateMachine.QueueFree();
    }

    private void TestMultipleSwitchesWhilePaused() {
        GD.Print("\n=== Test: Multiple Switches While Paused ===");
        _log.Clear();

        _stateMachine = new StateMachine();

        var state1 = new TestState("State1", _log);
        var state2 = new TestState("State2", _log);
        var state3 = new TestState("State3", _log);
        _stateMachine.AddChild(state1);
        _stateMachine.AddChild(state2);
        _stateMachine.AddChild(state3);

        AddChild(_stateMachine);

        _log.Clear();
        _stateMachine.IsPaused = true;
        _stateMachine.SwitchState(1); // State1 -> State2 (paused)
        _stateMachine.SwitchState(2); // State2 -> State3 (paused)

        AssertLog(new string[] { }, "No Entry/Exit while paused");
        Assert(_stateMachine.CurrentState.Name == "State3", "CurrentState should be State3");
        Assert(_stateMachine.PreviousState.Name == "State1", "PreviousState should still be State1");

        _log.Clear();
        _stateMachine.IsPaused = false;

        AssertLog(new[] {
            "State1.Exit(State3)",
            "State3.Entry(State1)"
        }, "On unpause, Exit from last entered state, Entry to current state");

        _stateMachine.QueueFree();
    }

    private void TestSwitchPauseSwitchUnpause() {
        GD.Print("\n=== Test: Switch, Pause, Switch, Unpause ===");
        _log.Clear();

        _stateMachine = new StateMachine();

        var state1 = new TestState("State1", _log);
        var state2 = new TestState("State2", _log);
        var state3 = new TestState("State3", _log);
        _stateMachine.AddChild(state1);
        _stateMachine.AddChild(state2);
        _stateMachine.AddChild(state3);

        AddChild(_stateMachine);

        _log.Clear();
        _stateMachine.SwitchState(1); // State1 -> State2 (not paused)

        AssertLog(new[] {
            "State1.Exit(State2)",
            "State2.Entry(State1)"
        }, "Normal switch should work");

        _log.Clear();
        _stateMachine.IsPaused = true;
        _stateMachine.SwitchState(2); // State2 -> State3 (paused)

        AssertLog(new string[] { }, "No Entry/Exit while paused");

        _log.Clear();
        _stateMachine.IsPaused = false;

        AssertLog(new[] {
            "State2.Exit(State3)",
            "State3.Entry(State2)"
        }, "On unpause, Exit from State2, Entry to State3");

        _stateMachine.QueueFree();
    }

    private void TestPauseUnpauseWithoutSwitch() {
        GD.Print("\n=== Test: Pause and Unpause Without Switching ===");
        _log.Clear();

        _stateMachine = new StateMachine();

        var state1 = new TestState("State1", _log);
        var state2 = new TestState("State2", _log);
        _stateMachine.AddChild(state1);
        _stateMachine.AddChild(state2);

        AddChild(_stateMachine);

        _log.Clear();
        _stateMachine.IsPaused = true;
        _stateMachine.IsPaused = false;

        AssertLog(new string[] { }, "Pause/Unpause without switching should not trigger Entry/Exit");

        _stateMachine.QueueFree();
    }

    private void AssertLog(string[] expected, string message) {
        bool match = _log.Count == expected.Length;
        if (match) {
            for (int i = 0; i < expected.Length; i++) {
                if (_log[i] != expected[i]) {
                    match = false;
                    break;
                }
            }
        }

        if (!match) {
            GD.PrintErr($"FAILED: {message}");
            GD.PrintErr($"  Expected: [{string.Join(", ", expected)}]");
            GD.PrintErr($"  Got:      [{string.Join(", ", _log)}]");
        } else {
            GD.Print($"✓ PASSED: {message}");
        }
    }

    private void Assert(bool condition, string message) {
        if (!condition) {
            GD.PrintErr($"FAILED: {message}");
        } else {
            GD.Print($"✓ PASSED: {message}");
        }
    }
}

public partial class TestState : State {
    private string _stateName;
    private List<string> _log;

    public TestState(string name, List<string> logRef) {
        Name = name;
        _stateName = name;
        _log = logRef;
    }

    public override void Entry(State previousState) {
        string prevName = previousState?.Name ?? "null";
        _log.Add($"{_stateName}.Entry({prevName})");
    }

    public override void Exit(State nextState) {
        string nextName = nextState?.Name ?? "null";
        _log.Add($"{_stateName}.Exit({nextName})");
    }
}
