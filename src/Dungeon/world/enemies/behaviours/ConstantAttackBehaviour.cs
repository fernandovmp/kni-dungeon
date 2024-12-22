using Dungeon.abstractions;
using Dungeon.world.characters.commands;
using Godot;

namespace Dungeon.world.enemies.behaviours;

[GlobalClass]
public partial class ConstantAttackBehaviour : BehaviourBase
{
    [Export] public double Interval { get; set; } = 3;
    private double _timer = 0;

    public override void OnPhysicsProcess(double delta, EnemyNode enemyNode)
    {
        _timer -= delta;
        if (_timer <= 0)
        {
            enemyNode.Character.Body.TryExecute(new CharacterAttackCommand());
            _timer = Interval;
        }
    }


}