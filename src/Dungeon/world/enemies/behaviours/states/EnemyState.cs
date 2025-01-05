using Dungeon.services.state_machine;
using Dungeon.world.player;
using Godot;

namespace Dungeon.world.enemies.behaviours.states;

[GlobalClass]
public abstract partial class EnemyState : State
{
    [Export] protected EnemyNode Enemy;
    protected PlayerNode Player;

    public override void Enter()
    {
        base.Enter();
        var node = GetTree().GetFirstNodeInGroup("Player");
        if (node is PlayerNode player)
        {
            Player = player;
        }
    }
}