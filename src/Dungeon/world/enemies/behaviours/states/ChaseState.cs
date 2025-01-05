using Dungeon.abstractions;
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
        if (_navigationAgent.IsNavigationFinished() || target.GlobalPosition.DistanceTo(Enemy.Character.Position) <= DistanceToTarget)
        {
            Enemy.Character.TryExecute(new CharacterMovementCommand(Vector2.Zero));
            return;
        }

        Vector2 currentAgentPosition = Enemy.Character.GlobalTransform.Origin;
        Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Vector2 direction = currentAgentPosition.DirectionTo(nextPathPosition);
        Enemy.Character.TryExecute(new CharacterMovementCommand(direction));
    }
}