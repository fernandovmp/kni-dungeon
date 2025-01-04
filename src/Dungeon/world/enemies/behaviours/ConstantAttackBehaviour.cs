using System.Collections.Generic;
using Dungeon.abstractions;
using Dungeon.world.characters.commands;
using Dungeon.world.player;
using Dungeon.world.weapons;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.enemies.behaviours;

[GlobalClass]
[Tool]
public partial class ConstantAttackBehaviour : BehaviourBase
{
    [Export] public double Interval { get; set; } = 3;
    private double _timer = 0;

    public override void OnPhysicsProcess(double delta, EnemyNode enemyNode)
    {
        _timer -= delta;
        if (_timer <= 0)
        {
            var weapon = enemyNode.Character.GetMetadata<WeaponNode>(nameof(WeaponNode));
            if (weapon != null && weapon.HasMeta("TargetPoint"))
            {
                if (enemyNode.BehaviorData.GetValueOrDefault("Target") is PlayerNode target)
                {
                    var targetPointPath = weapon.GetMetadata<NodePath>("TargetPoint");
                    var targetPoint = weapon.GetNode<Node2D>(targetPointPath);
                    targetPoint.Position = weapon.GlobalPosition.DirectionTo(target.Character.GlobalPosition);
                }
            }
            enemyNode.Character.TryExecute(new CharacterAttackCommand());
            _timer = Interval;
        }
    }


}