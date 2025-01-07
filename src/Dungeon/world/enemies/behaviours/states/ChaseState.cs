using Dungeon.abstractions;
using Dungeon.services.state_machine;
using Dungeon.world.characters.commands;
using Godot;

namespace Dungeon.world.enemies.behaviours.states;

[GlobalClass]
public partial class ChaseState : EnemyState
{
    [Export]
    public double DistanceToTarget { get; set; }

    [Export] private NavigationAgent2D _navigationAgent;
    protected bool NavigationReady;
    
    [Export]
    public State StateWhenReachTarget { get; private set; }

    public override void PhysicsUpdate(double delta)
    {
        base.PhysicsUpdate(delta);
        
        if (!NavigationReady)
        {
            NavigationReady = true;
            return;
        }

        if (Player == null)
        {
            return;
        }
        var target = Player.Character;

        if (_navigationAgent == null)
        {
            _navigationAgent = Enemy.Character.GetNodeOrNull<NavigationAgent2D>("NavigationAgent2D");
        }

        if (_navigationAgent == null)
        {
            return;
        }
        
        _navigationAgent.TargetPosition = target.GlobalPosition;
        if (_navigationAgent.IsNavigationFinished() || target.GlobalPosition.DistanceTo(Enemy.Character.GlobalPosition) <= DistanceToTarget)
        {
            Enemy.Character.TryExecute(new CharacterMovementCommand(Vector2.Zero));
            if (StateWhenReachTarget != null)
            {
                TransitionTo(StateWhenReachTarget.Name);
            }
            return;
        }

        Vector2 currentAgentPosition = Enemy.Character.GlobalTransform.Origin;
        Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Vector2 direction = currentAgentPosition.DirectionTo(nextPathPosition);
        // Vector2 direction =target.GlobalPosition -  Enemy.Character.GlobalPosition;
        // GD.Print(direction);
        Vector2 desiredDirection = Enemy.ContextMap.GetDesiredDirection(direction);
        Enemy.Character.TryExecute(new CharacterMovementCommand(desiredDirection));
    }
}