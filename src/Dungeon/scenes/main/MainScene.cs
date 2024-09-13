using Dungeon.ui.controls;
using Dungeon.world.arena;
using Dungeon.world.waves;
using FernandoVmp.GodotUtils.Scene;
using FernandoVmp.GodotUtils.Services;
using Godot;
using Godot.Collections;

namespace Dungeon.scenes.main;

public partial class MainScene : Node2D
{
    private ArenaNode _arenaNode;
    private ArenaData _arenaData;

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

        _arenaData = arenaData;

        GetNode<CurrentArena>("CanvasLayer/ArenaInfo/CurrentArena").SetArena(arenaData);
        ConfigureArena(arenaData);
        base._Ready();
    }

    private void ConfigureArena(ArenaData arenaData)
    {
        _arenaNode = GetNode<ArenaNode>("Arena");
        var waves = new Array<WaveResource>();
        foreach (var wave in arenaData.WavesResources)
        {
            waves.Add(ResourceLoader.Load<WaveResource>(wave));
        }
        _arenaNode.WavesResources = waves;
        _arenaNode.Level = ResourceLoader.Load<PackedScene>(arenaData.Level);
        _arenaNode.Configure();
    }

    public void OnWaveMessagePressed(WaveMessagePressedEvent @event)
    {
        if (@event.ArenaState == ArenaStateEnum.Setup)
        {
            _arenaNode.Start();
        }
        
        if (@event.ArenaState == ArenaStateEnum.Cleared)
        {
            ChangeArena();
        }
    }

    private void ChangeArena()
    {
        var cacheService = new MemoryCacheService();
        
        cacheService.AddOrReplace("ArenaData", new ArenaData()
        {
            Level = "res://world/dungeon/levels/level_00.tscn",
            WavesResources = ["res://world/waves/resources/debug_wave.tres", "res://world/waves/resources/debug_wave01.tres"],
            ArenaNumber = _arenaData.ArenaNumber + 1
        });
        SceneLoader.LoadInto(GetTree().Root, "res://scenes/main/main.tscn");
    }
}