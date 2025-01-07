using Dungeon.world.characters;
using Dungeon.world.enemies;
using Dungeon.world.enemies.behaviours;
using Godot;
using Godot.Collections;

namespace Dungeon.world.spawners;

public partial class SpawnerNode : Node2D
{

    public EnemyNode SpawnEnemy(PackedScene enemyScene, Node root)
    {
        var enemyNode = enemyScene.Instantiate<EnemyNode>();
        
        enemyNode.Visible = false;
        enemyNode.Ready += () =>
        {
            enemyNode.Character.GlobalPosition = GlobalPosition;
            enemyNode.Visible = true;
        };
        root.AddChild(enemyNode);
        return enemyNode;
    }
}