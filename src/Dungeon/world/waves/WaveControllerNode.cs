using System;
using System.Collections.Generic;
using Dungeon.world.characters;
using Dungeon.world.spawners;
using Godot;
using Godot.Collections;

namespace Dungeon.world.waves;

public partial class WaveControllerNode : Node2D
{
    [Export] public WaveResource CurrentWaveResource { get; set; }
    [Export] public Array<WaveResource> WavesResources { get; set; }
    [Export] public SpawnPoolNode SpawnPool { get; set; }
    [Export] private double _minimumInterval = 3;
    [Export] private int _maxEnemyCount = 7;
    public int WaveNumber => _currentWaveIndex + 1;
    
    private int _currentWaveIndex = -1;
    private double _timer;
    private Queue<PackedScene> _enemiesQueue = new Queue<PackedScene>(0);
    private bool _notNotifiedWaveEnd;

    [Signal] public delegate void OnWaveEndedEventHandler();
    [Signal] public delegate void OnWaveChangedEventHandler(WaveChangedEvent @event);

    public partial class WaveChangedEvent : GodotObject
    {
        public int WaveNumber { get; set; }
        public WaveResource Wave { get; set; }
    }

    [Signal] public delegate void OnFinishedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        _timer = _minimumInterval;

        if (_currentWaveIndex == -1 && WavesResources != null && WavesResources.Count > 0)
        {
            NextWave();
        }
    }
    
    public void NextWave()
    {
        _currentWaveIndex++;
        if (_currentWaveIndex >= WavesResources.Count)
        {
            EmitSignal(SignalName.OnFinished);
            return;
        }
        CurrentWaveResource = WavesResources[_currentWaveIndex];
        StartWave();
        
        EmitSignal(SignalName.OnWaveChanged, new WaveChangedEvent
        {
            Wave = CurrentWaveResource,
            WaveNumber = _currentWaveIndex + 1
        });
    }
    
    public void StartWave()
    {
        _enemiesQueue = new Queue<PackedScene>(CurrentWaveResource!.Enemies);
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
            var enemy = _enemiesQueue.Dequeue();
            SpawnPool.SpawnEnemy(enemy);
            enemyCount += 1;
            _timer = _minimumInterval;
        }

        if (enemyCount == 0 && _enemiesQueue.Count == 0 && _notNotifiedWaveEnd)
        {
            _notNotifiedWaveEnd = false;
            NextWave();
            EmitSignal(SignalName.OnWaveEnded);
        }
    }
}