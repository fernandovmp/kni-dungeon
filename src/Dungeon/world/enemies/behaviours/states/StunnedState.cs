using Dungeon.abstractions;
using Dungeon.services.state_machine;
using Dungeon.world.characters.commands;
using Godot;

namespace Dungeon.world.enemies.behaviours.states;

[GlobalClass]
public partial class StunnedState : EnemyState
{
    private bool _connected;
    private bool _enabled;
    private bool _isStunned;
    private double _timer;
    
    [Export]
    public double Duration { get; set; }
    
    [Export]
    public State StateWhenFinished { get; private set; }
    
    public override void Enter()
    {
        base.Enter();
        _isStunned = true;
        _timer = Duration;
        Enemy.Character.TryExecute(new CharacterMovementCommand(Vector2.Zero));
    }

    public override void Exit()
    {
        base.Exit();
        _isStunned = false;
    }

    public override void Update(double delta)
    {
        base.Update(delta);
        if (!_isStunned) return;
        
        _timer -= delta;
        if (_timer <= 0 && StateWhenFinished != null)
        {
            TransitionTo(StateWhenFinished.Name);
        }
    }
}