using Godot;

namespace Dungeon.services.state_machine;

[GlobalClass]
public abstract partial class State : Node
{
    [Signal]
    public delegate void TranstionedEventHandler(string state);
    
    protected void TransitionTo(string state) => EmitSignal(SignalName.Transtioned, state);

    public virtual void Enter()
    {
    }
    
    public virtual void Exit()
    {
    }
    
    
    public virtual void Update(double delta)
    {
    }

    public virtual void PhysicsUpdate(double delta)
    {
    }
}