using Dungeon.world.arena;
using Dungeon.world.waves;
using FernandoVmp.GodotUtils.Services;
using Godot;
using Godot.Collections;

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
                WavesResources = ["res://world/waves/resources/debug_wave.tres", "res://world/waves/resources/debug_wave01.tres"]
            };
#endif
        }

        var arenaNode = GetNode<ArenaNode>("Arena");
        var waves = new Array<WaveResource>();
        foreach (var wave in arenaData.WavesResources)
        {
            waves.Add(ResourceLoader.Load<WaveResource>(wave));
        }
        arenaNode.WavesResources = waves;
        arenaNode.Level = ResourceLoader.Load<PackedScene>(arenaData.Level);
        arenaNode.Configure();
        base._Ready();
    }
}