using Godot;
using System;
using Blueberry;
using System.Collections.Generic;

public partial class StateMachineExample : Node {
    private StateMachine StateMachine;
    private List<string> Log = new List<string>();

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
        Log.Clear();

        StateMachine = new StateMachine();

        var state1 = new TestState("State1", Log);
        var state2 = new TestState("State2", Log);
        StateMachine.AddChild(state1);
        StateMachine.AddChild(state2);

        AddChild(StateMachine);

        AssertLog(new[] { "State1.Entry(null)" }, "Initial state should have Entry called");

        StateMachine.QueueFree();
    }

    private void TestBasicStateSwitching() {
        GD.Print("\n=== Test: Basic State Switching ===");
        Log.Clear();

        StateMachine = new StateMachine();

        var state1 = new TestState("State1", Log);
        var state2 = new TestState("State2", Log);
        StateMachine.AddChild(state1);
        StateMachine.AddChild(state2);

        AddChild(StateMachine);

        StateMachine.SwitchState(1);

        AssertLog(new[] {
            "State1.Entry(null)",
            "State1.Exit(State2)",
            "State2.Entry(State1)"
        }, "State switch should call Exit then Entry");

        StateMachine.QueueFree();
    }

    private void TestStartPausedSwitchUnpause() {
        GD.Print("\n=== Test: Start Paused, Switch While Paused, Unpause ===");
        Log.Clear();

        StateMachine = new StateMachine();
        StateMachine.IsPaused = true;

        var state1 = new TestState("State1", Log);
        var state2 = new TestState("State2", Log);
        StateMachine.AddChild(state1);
        StateMachine.AddChild(state2);

        AddChild(StateMachine);

        AssertLog(new string[] { }, "No Entry should be called when starting paused");

        Log.Clear();
        StateMachine.SwitchState(1);

        AssertLog(new string[] { }, "No Entry/Exit while paused");

        Log.Clear();
        StateMachine.IsPaused = false;

        AssertLog(new[] {
            "State2.Entry(null)"
        }, "On unpause, only Entry on current state (State1 never entered, so no Exit)");

        StateMachine.QueueFree();
    }

    private void TestMultipleSwitchesWhilePaused() {
        GD.Print("\n=== Test: Multiple Switches While Paused ===");
        Log.Clear();

        StateMachine = new StateMachine();

        var state1 = new TestState("State1", Log);
        var state2 = new TestState("State2", Log);
        var state3 = new TestState("State3", Log);
        StateMachine.AddChild(state1);
        StateMachine.AddChild(state2);
        StateMachine.AddChild(state3);

        AddChild(StateMachine);

        Log.Clear();
        StateMachine.IsPaused = true;
        StateMachine.SwitchState(1); // State1 -> State2 (paused)
        StateMachine.SwitchState(2); // State2 -> State3 (paused)

        AssertLog(new string[] { }, "No Entry/Exit while paused");
        Assert(StateMachine.CurrentState.Name == "State3", "CurrentState should be State3");
        Assert(StateMachine.PreviousState.Name == "State1", "PreviousState should still be State1");

        Log.Clear();
        StateMachine.IsPaused = false;

        AssertLog(new[] {
            "State1.Exit(State3)",
            "State3.Entry(State1)"
        }, "On unpause, Exit from last entered state, Entry to current state");

        StateMachine.QueueFree();
    }

    private void TestSwitchPauseSwitchUnpause() {
        GD.Print("\n=== Test: Switch, Pause, Switch, Unpause ===");
        Log.Clear();

        StateMachine = new StateMachine();

        var state1 = new TestState("State1", Log);
        var state2 = new TestState("State2", Log);
        var state3 = new TestState("State3", Log);
        StateMachine.AddChild(state1);
        StateMachine.AddChild(state2);
        StateMachine.AddChild(state3);

        AddChild(StateMachine);

        Log.Clear();
        StateMachine.SwitchState(1); // State1 -> State2 (not paused)

        AssertLog(new[] {
            "State1.Exit(State2)",
            "State2.Entry(State1)"
        }, "Normal switch should work");

        Log.Clear();
        StateMachine.IsPaused = true;
        StateMachine.SwitchState(2); // State2 -> State3 (paused)

        AssertLog(new string[] { }, "No Entry/Exit while paused");

        Log.Clear();
        StateMachine.IsPaused = false;

        AssertLog(new[] {
            "State2.Exit(State3)",
            "State3.Entry(State2)"
        }, "On unpause, Exit from State2, Entry to State3");

        StateMachine.QueueFree();
    }

    private void TestPauseUnpauseWithoutSwitch() {
        GD.Print("\n=== Test: Pause and Unpause Without Switching ===");
        Log.Clear();

        StateMachine = new StateMachine();

        var state1 = new TestState("State1", Log);
        var state2 = new TestState("State2", Log);
        StateMachine.AddChild(state1);
        StateMachine.AddChild(state2);

        AddChild(StateMachine);

        Log.Clear();
        StateMachine.IsPaused = true;
        StateMachine.IsPaused = false;

        AssertLog(new string[] { }, "Pause/Unpause without switching should not trigger Entry/Exit");

        StateMachine.QueueFree();
    }

    private void AssertLog(string[] expected, string message) {
        bool match = Log.Count == expected.Length;
        if (match) {
            for (int i = 0; i < expected.Length; i++) {
                if (Log[i] != expected[i]) {
                    match = false;
                    break;
                }
            }
        }

        if (!match) {
            GD.PrintErr($"FAILED: {message}");
            GD.PrintErr($"  Expected: [{string.Join(", ", expected)}]");
            GD.PrintErr($"  Got:      [{string.Join(", ", Log)}]");
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
    private string StateName;
    private List<string> Log;

    public TestState(string name, List<string> logRef) {
        Name = name;
        StateName = name;
        Log = logRef;
    }

    public override void Entry(State previousState) {
        string prevName = previousState?.Name ?? "null";
        Log.Add($"{StateName}.Entry({prevName})");
    }

    public override void Exit(State nextState) {
        string nextName = nextState?.Name ?? "null";
        Log.Add($"{StateName}.Exit({nextName})");
    }
}
