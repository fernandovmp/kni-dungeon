using Dungeon.world.characters.commands;
using Dungeon.world.player;
using Godot;

namespace Dungeon.world.enemies.behaviours;

[GlobalClass]
public partial class ChaseBehaviour : BehaviourBase
{
    private PlayerNode _target;
    private NavigationAgent2D _navigationAgent;

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

        if (_navigationAgent == null)
        {
            _navigationAgent = body.GetNodeOrNull<NavigationAgent2D>("NavigationAgent2D");
        }

        if (_navigationAgent == null)
        {
            return;
        }
        
        _navigationAgent.TargetPosition = _target.Character.Body.GlobalPosition;
        if (_navigationAgent.IsNavigationFinished())
        {
            enemyNode.Character.Execute(new CharacterMovementCommand(Vector2.Zero));
            return;
        }

        Vector2 currentAgentPosition = body.GlobalTransform.Origin;
        Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Vector2 direction = currentAgentPosition.DirectionTo(nextPathPosition);
        enemyNode.Character.Execute(new CharacterMovementCommand(direction));
    }
}