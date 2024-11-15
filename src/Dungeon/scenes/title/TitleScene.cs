using Dungeon.scenes.main;
using Dungeon.world;
using FernandoVmp.GodotUtils.Scene;
using FernandoVmp.GodotUtils.Services;
using Godot;

namespace Dungeon.scenes.title;

public partial class TitleScene : Node2D
{
    private Control _mainUi;
    private Control _creditsUi;
    
    public override void _Ready()
    {
        _mainUi = GetNode<Control>("CanvasLayer/Main");
        _creditsUi = GetNode<Control>("CanvasLayer/Credits");
        
        using var file = FileAccess.Open("res://credits.txt", FileAccess.ModeFlags.Read);
        var credits = file.GetAsText();
        var label = _creditsUi.GetNode<Label>("VBoxContainer/Panel/ScrollContainer/Label");
        label.Text = credits;
    }

    public void Start()
    {
        var cacheService = new MemoryCacheService();
        var arenaData = ArenaDefinitionListResource.S_CreateArena(0, null);
        arenaData.ArenaNumber = 1;
        
        cacheService.AddOrReplace("ArenaData", arenaData);
        ProgressMonitorNode.S_Reset();
        SceneLoader.LoadInto(GetTree().Root, "res://scenes/main/main.tscn");
    }

    public void Credits()
    {
        _mainUi.Visible = false;
        _creditsUi.Visible = true;
    }

    public void CloseCredits()
    {
        _mainUi.Visible = true;
        _creditsUi.Visible = false;
    }
    
    public void Quit()
    {
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
        GetTree().Quit();
    }
}