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
    
    [Signal]
    public delegate void ArenaStateChangedEventHandler(ArenaState state);
    
    [Signal]
    public delegate void OnArenaEnemyDiedEventHandler();

    private int _currentWaveIndex;
    private Node2D _map;
    private WaveControllerNode _waveController;

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
        _waveController.SpawnPool.Connect(SpawnPoolNode.SignalName.OnEnemyDied, new Callable(this, nameof(OnEnemyDied)));
        
        EmitSignal(SignalName.ArenaStateChanged,
            new ArenaState(null, -1, ArenaStateEnum.Setup));
    }

    public void Start()
    {
        UpdateCurrentWave();
    }

    private void OnWaveEnd()
    {
        _currentWaveIndex++;
        if (_currentWaveIndex >= WavesResources.Count)
        {
            EmitSignal(SignalName.ArenaStateChanged,
                new ArenaState(null, -1, ArenaStateEnum.Cleared));
            return;
        }
        UpdateCurrentWave();
    }
    
    private void UpdateCurrentWave()
    {
        CurrentWaveResource = WavesResources[_currentWaveIndex];
        _waveController.Configure(CurrentWaveResource);
        EmitSignal(SignalName.ArenaStateChanged,
            new ArenaState(CurrentWaveResource, _currentWaveIndex, ArenaStateEnum.WaveChange));
    }

    public void OnEnemyDied()
    {
        GD.Print("ArenaNode");
        EmitSignal(SignalName.OnArenaEnemyDied);
    }
}

public partial class ArenaState(WaveResource wave, int index, ArenaStateEnum state) : GodotObject
{
    public WaveResource Wave { get; } = wave;
    public int Index { get; } = index;
    public ArenaStateEnum State { get; } = state;
}

public enum ArenaStateEnum
{
    None,
    Setup,
    WaveChange,
    Cleared
}