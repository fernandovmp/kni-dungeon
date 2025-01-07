using System;
using System.Collections.Generic;
using Dungeon.world.characters;
using Dungeon.world.enemies;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.spawners;

public partial class SpawnPoolNode : Node2D
{
    private Node2D _enemiesRoot;
    private List<SpawnerNode> _spawnersNodes = new List<SpawnerNode>();
    private int _lastSpawnerIndex = -1;
    private readonly Random _random = new Random();
    
    [Signal]
    public delegate void OnEnemySpawnEventHandler(EnemyNode enemy);

    public override void _Ready()
    {
        base._Ready();
        _enemiesRoot = GetNode<Node2D>("EnemiesRoot");
        FindSpawners();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        Owner.SetMetadata(nameof(SpawnPoolNode), this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Owner.RemoveMeta(nameof(SpawnPoolNode));
    }

    private void FindSpawners()
    {
        var children = GetChildren();
        foreach (var child in children)
        {
            if (child is SpawnerNode spwaner)
            {
                _spawnersNodes.Add(spwaner);
            }
        }
    }

    public void SpawnEnemy(PackedScene enemyScene)
    {
        if (_spawnersNodes.Count == 0)
        {
            return;
        }

        int index = 0;
        while (index == _lastSpawnerIndex && _spawnersNodes.Count > 1)
        {
            index = _random.Next(0, _spawnersNodes.Count);
        }
        var spawner = _spawnersNodes[index];
        _lastSpawnerIndex = index;
        var enemy = spawner.SpawnEnemy(enemyScene, _enemiesRoot);
        EmitSignal(SignalName.OnEnemySpawn, enemy);
    }

    public int CountActiveEnemies() => _enemiesRoot.GetChildCount();
}