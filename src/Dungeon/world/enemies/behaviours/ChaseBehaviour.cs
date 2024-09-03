using Dungeon.world.characters.commands;
using Dungeon.world.player;
using Godot;

namespace Dungeon.world.enemies.behaviours;

[GlobalClass]
public partial class ChaseBehaviour : BehaviourBase
{
    private PlayerNode _target;

    public override void OnPhysicsProcess(double delta, EnemyNode enemyNode)
    {
        var body = enemyNode.Character.Body;
        if (_target == null)
        {
            _target = enemyNode.GetTree().Root.FindChild("Player", recursive: true, owned: false)  as PlayerNode;
            if (_target == null)
            {
                return;
            }
        }

        if (!IsInstanceValid(_target) || !IsInstanceValid(_target.Character) ||
            !IsInstanceValid(_target.Character.Body))
        {
            _target = null;
            return;
        }
        body.NavigationAgent.TargetPosition = _target.Character.Body.GlobalPosition;
        if (body.NavigationAgent.IsNavigationFinished())
        {
            enemyNode.Character.Execute(new CharacterMovementCommand(Vector2.Zero));
            return;
        }

        Vector2 currentAgentPosition = body.GlobalTransform.Origin;
        Vector2 nextPathPosition = body.NavigationAgent.GetNextPathPosition();
        Vector2 direction = currentAgentPosition.DirectionTo(nextPathPosition);
        enemyNode.Character.Execute(new CharacterMovementCommand(direction));
    }
}