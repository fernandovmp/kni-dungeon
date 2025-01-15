using Dungeon.services;
using Dungeon.ui.controls;
using Dungeon.world;
using Dungeon.world.arena;
using Dungeon.world.characters.components;
using Dungeon.world.player;
using FernandoVmp.GodotUtils.Extensions;
using FernandoVmp.GodotUtils.Scene;
using FernandoVmp.GodotUtils.Services;
using Godot;

namespace Dungeon.scenes.main;

public partial class MainScene : Node2D
{
    private ArenaNode _arenaNode;
    private ArenaData _arenaData;
    
    private Control _mainUI;
    private ArenaResultPanel _resultsUI;
    private AudioStreamPlayer2D _backgroundMusic;

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

        _backgroundMusic = GetNode<AudioStreamPlayer2D>("BackgroundMusic");
        ConfigureUI(arenaData);
        ConfigureArena(arenaData);
        base._Ready();
    }

    private void ConfigureUI(ArenaData arenaData)
    {
        GetNode<CurrentArena>("CanvasLayer/MainUI/ArenaInfo/CurrentArena").SetArena(arenaData);
        _mainUI = GetNode<Control>("CanvasLayer/MainUI");
        _resultsUI = GetNode<ArenaResultPanel>("CanvasLayer/ResultsUI");
        ShowUI(UIRoutesEnum.Main);
    }

    public void OnPlayerDied(PlayerNode playerNode)
    {
        ShowUI(UIRoutesEnum.Death);
    }

    private void ShowUI(UIRoutesEnum route)
    {
        _mainUI.Visible = route == UIRoutesEnum.Main;
        if (route == UIRoutesEnum.Cleared || route == UIRoutesEnum.Death)
        {
            var progressMonitor = GetNode<ProgressMonitorNode>("ProgressMonitor");
            var player = GetNode<PlayerNode>("Player");
            var progress = progressMonitor.GetProgress();
            var combatent = player.Character.GetMetadata<CombatentNode>(nameof(CombatentNode));
            progress.CurrentLife = combatent.Life;
            _backgroundMusic.Stop();
            ShowResults(route, progress);
        }
        else
        {
            _resultsUI.Hide();
        }
    }

    private void ShowResults(UIRoutesEnum route, ProgressData progress)
    {
        if (route == UIRoutesEnum.Cleared)
        {
            _resultsUI.ShowCleared(progress);
            return;
        }
        _resultsUI.ShowDeath(progress);
    }

    private void ConfigureArena(ArenaData arenaData)
    {
        _arenaNode = GetNode<ArenaNode>("Arena");
        _arenaNode.Configure(arenaData);
        var node = GetTree().GetFirstNodeInGroup("Player");
        var originNode = GetTree().GetFirstNodeInGroup("PlayerOrigin");
        if (node is PlayerNode player && originNode is Node2D origin)
        {
            player.Character.GlobalPosition = origin.GlobalPosition;
        }
    }

    public void OnWaveMessagePressed()
    {
        if (_arenaNode.State == ArenaStateEnum.Setup)
        {
            _arenaNode.WaveController.NextWave();
        }
        
        if (@_arenaNode.State == ArenaStateEnum.Cleared)
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