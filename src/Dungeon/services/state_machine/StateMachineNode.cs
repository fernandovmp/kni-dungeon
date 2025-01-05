using System;
using System.Collections.Generic;
using Godot;

namespace Dungeon.services.state_machine;

[GlobalClass]
public partial class StateMachineNode : Node
{
    private Dictionary<string, State> _states = new Dictionary<string, State>(StringComparer.OrdinalIgnoreCase);
    public State CurrentState { get; private set; }
    [Export]
    public State InitialState { get; set; }
    
    public override void _Ready()
    {
        base._Ready();
        foreach (var child in GetChildren())
        {
            if (child is State state)
            {
                state.Connect(State.SignalName.Transtioned, new Callable(this, nameof(OnTransitioned)));
                _states.Add(state.Name, state);
            }
        }
        CurrentState = InitialState;
        CurrentState?.Enter();
    }
    
    private void OnTransitioned(string state)
    {
        if (state.Equals(CurrentState?.Name, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        var nextState = _states.GetValueOrDefault(state);
        if (nextState == null)
        {
            return;
        }
        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        CurrentState?.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        CurrentState?.PhysicsUpdate(delta);
    }
}
