using System;
using System.Collections.Generic;
using Dungeon.world.characters;
using Dungeon.world.spawners;
using Godot;

namespace Dungeon.world.waves;

public partial class WaveControllerNode : Node2D
{
    [Export] public SpawnPoolNode SpawnPool { get; set; }
    [Export] private double _minimumInterval = 3;
    [Export] private int _maxEnemyCount = 7;
    [Export] private WaveResource? _wave;
    private double _timer;
    private Queue<CharacterResource> _enemiesQueue = new Queue<CharacterResource>(0);
    private bool _notNotifiedWaveEnd;

    public Action OnWaveEnd { get; set; }

    public override void _Ready()
    {
        base._Ready();
        _timer = _minimumInterval;

        if (_wave != null)
        {
            Configure(_wave);
        }
    }

    public void Configure(WaveResource wave)
    {
        _wave = wave;
        _enemiesQueue = new Queue<CharacterResource>(_wave!.Enemies);
        _notNotifiedWaveEnd = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (SpawnPool == null)
        {
            return;
        }
        int enemyCount = SpawnPool.CountActiveEnemies();
        if (enemyCount < _maxEnemyCount)
        {
            _timer -= delta;
        }
        if (_timer <= 0 && _enemiesQueue.Count > 0)
        {
            var character = _enemiesQueue.Dequeue();
            SpawnPool.SpawnEnemy(character);
            _timer = _minimumInterval;
        }

        if (enemyCount == 0 && _enemiesQueue.Count == 0 && _notNotifiedWaveEnd)
        {
            _notNotifiedWaveEnd = false;
            OnWaveEnd?.Invoke();
        }
    }
}