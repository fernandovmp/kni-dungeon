using System.Collections.Generic;
using Dungeon.world.characters;
using Dungeon.world.enemies;
using Dungeon.world.enemies.behaviours;
using Godot;
using Godot.Collections;

namespace Dungeon.world.spawners;

public partial class SpawnerNode : Node2D
{
    private PackedScene _enemyScene;
    
    public override void _Ready()
    {
        _enemyScene = ResourceLoader.Load<PackedScene>("res://world/enemies/enemy.tscn");
    }

    private double timer = 3;
    public override void _PhysicsProcess(double delta)
    {
        timer -= delta;
        if (timer <= 0)
        {
            SpawnEnemy(ResourceLoader.Load<CharacterResource>("res://world/characters/orc_warrior/character.tres"));
            timer = 3;
        }
    }

    public bool SpawnEnemy(CharacterResource character)
    {
        var enemyNode = _enemyScene.Instantiate<EnemyNode>();
        enemyNode.Behaviour = new ComposedBehaviour()
        {
            BehavioursResources = new Array<Resource>(new Resource[]
            {
                new ChaseBehaviour(), new ConstantAttackBehaviour()
            })
        };
        enemyNode.CharacterResource = character;
        enemyNode.Visible = false;
        enemyNode.Ready += () =>
        {
            enemyNode.Character.Body.GlobalPosition = GlobalPosition;
            enemyNode.Visible = true;
        };
        AddChild(enemyNode);
        return true;
    }
}