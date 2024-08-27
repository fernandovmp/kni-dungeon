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

    public bool SpawnEnemy(CharacterResource character, Node root)
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
        root.AddChild(enemyNode);
        return true;
    }
}