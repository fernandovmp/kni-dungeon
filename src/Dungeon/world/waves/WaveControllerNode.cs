using System.Collections.Generic;
using Dungeon.world.characters;
using Dungeon.world.spawners;
using Godot;

namespace Dungeon.world.waves;

public partial class WaveControllerNode : Node2D
{
    [Export] private SpawnPoolNode _spawnPool;
    [Export] private double _minimumInterval = 3;
    [Export] private int _maxEnemyCount = 7;
    [Export] private WaveResource _wave;
    private double _timer;
    private Queue<CharacterResource> _enemiesQueue;

    public override void _Ready()
    {
        base._Ready();
        _timer = _minimumInterval;
        _enemiesQueue = new Queue<CharacterResource>(_wave.Enemies);
    }

    public override void _PhysicsProcess(double delta)
    {
        int enemyCount = _spawnPool.CountActiveEnemies();
        if (enemyCount < _maxEnemyCount)
        {
            _timer -= delta;
        }
        if (_timer <= 0 && _enemiesQueue.Count > 0)
        {
            var character = _enemiesQueue.Dequeue();
            _spawnPool.SpawnEnemy(character);
            _timer = _minimumInterval;
        }
    }
}