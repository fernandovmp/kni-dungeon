using Dungeon.ui.controls;
using Dungeon.world;
using Dungeon.world.arena;
using Dungeon.world.player;
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
    
    private Control _mainUI;
    private Control _deathUI;
    private Control _clearedUI;

    private enum UIRoutesEnum
    {
        Main,
        Death,
        Cleared
    }

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

        ConfigureUI(arenaData);
        ConfigureArena(arenaData);
        base._Ready();
    }

    private void ConfigureUI(ArenaData arenaData)
    {
        GetNode<CurrentArena>("CanvasLayer/MainUI/ArenaInfo/CurrentArena").SetArena(arenaData);
        _mainUI = GetNode<Control>("CanvasLayer/MainUI");
        _deathUI = GetNode<Control>("CanvasLayer/DeathUI");
        _clearedUI = GetNode<Control>("CanvasLayer/ClearedUI");
        ShowUI(UIRoutesEnum.Main);
    }

    public void OnPlayerDied(PlayerNode playerNode)
    {
        ShowUI(UIRoutesEnum.Death);
    }

    private void ShowUI(UIRoutesEnum route)
    {
        _mainUI.Visible = route == UIRoutesEnum.Main;
        _deathUI.Visible = route == UIRoutesEnum.Death;
        _clearedUI.Visible = route == UIRoutesEnum.Cleared;
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
            if (_arenaData.ArenaNumber >= 3)
            {
                ShowUI(UIRoutesEnum.Cleared);
            }
            else
            {
                ChangeArena();
            }
        }
    }

    private void ChangeArena()
    {
        var cacheService = new MemoryCacheService();
        
        var arenaData = ArenaDefinitionListResource.S_CreateArena(_arenaData.ArenaNumber, _arenaData);
        arenaData.ArenaNumber = _arenaData.ArenaNumber + 1;
        cacheService.AddOrReplace("ArenaData", arenaData);
        SceneLoader.LoadInto(GetTree().Root, "res://scenes/main/main.tscn");
    }

    public void Retry()
    {
        SceneLoader.LoadInto(GetTree().Root, "res://scenes/main/main.tscn");
    }

    public void GoToTitle()
    {
        SceneLoader.LoadInto(GetTree().Root, "res://scenes/title/title.tscn");
    }
}