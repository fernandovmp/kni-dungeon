using Dungeon.world.arena;
using Dungeon.world.waves;
using FernandoVmp.GodotUtils.Services;
using Godot;

namespace Dungeon.scenes.main;

public partial class MainScene : Node2D
{
    public override void _Ready()
    {
        var cacheService = new MemoryCacheService();
        var arenaData = cacheService.GetValueOrDefault<ArenaData>("ArenaData");
        if (arenaData == null)
        {
#if DEBUG
            arenaData = new ArenaData
            {
                Level = "res://world/dungeon/levels/level_00.tscn",
                WaveResource = "res://world/waves/resources/debug_wave.tres"
            };
#endif
        }

        var arenaNode = GetNode<ArenaNode>("Arena");
        arenaNode.WaveResource = ResourceLoader.Load<WaveResource>(arenaData.WaveResource);
        arenaNode.Level = ResourceLoader.Load<PackedScene>(arenaData.Level);
        arenaNode.Configure();
        base._Ready();
    }
}