using Dungeon.world.spawners;
using Dungeon.world.waves;
using Godot;

namespace Dungeon.world.arena;

public partial class ArenaNode : Node2D
{
    [Export] public PackedScene Level { get; set; }
    [Export] public WaveResource WaveResource { get; set; }

    private Node2D _map;
    private WaveControllerNode _waveController;

    public override void _Ready()
    {
        Configure();
        base._Ready();
    }

    public void Configure()
    {
        if (Level == null || WaveResource == null)
        {
            return;
        }
        _map = GetNode<Node2D>("Map");
        var level = Level.Instantiate();
        _map.AddChild(level);

        _waveController = GetNode<WaveControllerNode>("WaveController");
        _waveController.Configure(WaveResource);
        _waveController.SpawnPool = level.GetNode<SpawnPoolNode>("SpawnPool");
    }
}