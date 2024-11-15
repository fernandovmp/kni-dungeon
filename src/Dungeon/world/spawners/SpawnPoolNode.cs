using System;
using System.Collections.Generic;
using Dungeon.world.characters;
using Dungeon.world.enemies;
using Godot;

namespace Dungeon.world.spawners;

public partial class SpawnPoolNode : Node2D
{
    private Node2D _enemiesRoot;
    private List<SpawnerNode> _spawnersNodes = new List<SpawnerNode>();
    private int _lastSpawnerIndex = -1;
    private readonly Random _random = new Random();
    
    [Signal]
    public delegate void OnEnemyDiedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        _enemiesRoot = GetNode<Node2D>("EnemiesRoot");
        FindSpawners();
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

    public void SpawnEnemy(CharacterResource character)
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
        var enemy = spawner.SpawnEnemy(character, _enemiesRoot);
        enemy.Connect(EnemyNode.SignalName.OnDied, new Callable(this, nameof(EmitEnemyDied)));
    }

    private void EmitEnemyDied()
    {
        EmitSignal(SignalName.OnEnemyDied);
    }

    public int CountActiveEnemies() => _enemiesRoot.GetChildCount();
}