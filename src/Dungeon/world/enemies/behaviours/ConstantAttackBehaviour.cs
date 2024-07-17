using Dungeon.world.characters.commands;
using Godot;

namespace Dungeon.world.enemies.behaviours;

public partial class ConstantAttackBehaviour : Resource, IEnemyBehaviour
{
    [Export] public double Interval { get; set; } = 3;
    private double _timer = 0;

    public void OnPhysicsProcess(double delta, EnemyNode enemyNode)
    {
        _timer -= delta;
        if (_timer <= 0)
        {
            var command = new CharacterAttackCommand();
            command.Execute(enemyNode.Character);
            _timer = Interval;
        }
    }


}