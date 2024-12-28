using Dungeon.world.enemies;
using Dungeon.world.spawners;
using Dungeon.world.waves;
using Godot;
using Godot.Collections;

namespace Dungeon.world.arena;

public partial class ArenaNode : Node2D
{
    [Export] public PackedScene Level { get; set; }
    
    [Signal]
    public delegate void ArenaStateChangedEventHandler(ArenaState state);
    
    public WaveControllerNode WaveController { get; private set; }
    public ArenaStateEnum State { get; private set; }
    
    [Signal]
    public delegate void OnArenaEnemyDiedEventHandler();

    private int _currentWaveIndex;
    private Node2D _map;

    public void Configure(ArenaData arenaData)
    {
        var waves = new Array<WaveResource>();
        foreach (var wave in arenaData.WavesResources)
        {
            waves.Add(ResourceLoader.Load<WaveResource>(wave));
        }
        Level = ResourceLoader.Load<PackedScene>(arenaData.Level);
        if (Level == null || waves.Count == 0)
        {
            return;
        }
        _map = GetNode<Node2D>("Map");
        var level = Level.Instantiate();
        _map.AddChild(level);

        WaveController = GetNode<WaveControllerNode>("WaveController");
        WaveController.SpawnPool = level.GetNode<SpawnPoolNode>("SpawnPool");
        WaveController.WavesResources = waves;

        WaveController.SpawnPool.Connect(SpawnPoolNode.SignalName.OnEnemySpawn,
            new Callable(this, nameof(OnEnemySpawned)));
        ChangeState(ArenaStateEnum.Setup);
    }

    public void OnWaveChange(WaveControllerNode.WaveChangedEvent @event)
    {
        ChangeState(ArenaStateEnum.WaveChange);
    }

    public void OnWavesFinished()
    {
        ChangeState(ArenaStateEnum.Cleared);
    }

    private void ChangeState(ArenaStateEnum state)
    {
        State = state;
        EmitSignal(SignalName.ArenaStateChanged,
            new ArenaState(State, WaveController.WaveNumber));
    }

    private void OnEnemySpawned(EnemyNode enemyNode)
    {
        enemyNode.Connect(EnemyNode.SignalName.OnDied, new Callable(this, nameof(OnEnemyDied)));
    }

    public void OnEnemyDied()
    {
        EmitSignal(SignalName.OnArenaEnemyDied);
    }
}
