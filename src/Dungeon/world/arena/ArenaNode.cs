using Dungeon.world.spawners;
using Dungeon.world.waves;
using Godot;
using Godot.Collections;

namespace Dungeon.world.arena;

public partial class ArenaNode : Node2D
{
    [Export] public PackedScene Level { get; set; }
    [Export] public WaveResource CurrentWaveResource { get; set; }
    [Export] public Array<WaveResource> WavesResources { get; set; }

    private int _currentWaveIndex;
    private Node2D _map;
    private WaveControllerNode _waveController;

    public override void _Ready()
    {
        Configure();
        base._Ready();
    }

    public void Configure()
    {
        if (Level == null || WavesResources == null || WavesResources.Count == 0)
        {
            return;
        }
        _map = GetNode<Node2D>("Map");
        var level = Level.Instantiate();
        _map.AddChild(level);

        _waveController = GetNode<WaveControllerNode>("WaveController");
        _waveController.OnWaveEnd += OnWaveEnd;
        _waveController.SpawnPool = level.GetNode<SpawnPoolNode>("SpawnPool");
        UpdateCurrentWave();
    }

    private void OnWaveEnd()
    {
        _currentWaveIndex++;
        if (_currentWaveIndex >= WavesResources.Count)
        {
            return;
        }
        UpdateCurrentWave();
    }
    
    private void UpdateCurrentWave()
    {
        CurrentWaveResource = WavesResources[_currentWaveIndex];
        _waveController.Configure(CurrentWaveResource);
    }
}