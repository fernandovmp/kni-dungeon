using System;
using Dungeon.world.arena;
using Godot;

namespace Dungeon.ui.controls;

public partial class WaveMessage : Panel
{
    private Label _label;
    private Label _pressSpaceLabel;
    private ArenaStateEnum _currentState;
    private double _timer = 4;
    private bool _awaitConfirmation;
    
    [Signal]
    public delegate void MessagePressedEventHandler();

    public bool IsVisible 
    { 
        get => _label?.Visible ?? false; 
        private set 
        {
            if (_label == null)
            {
                throw new Exception("WaveMessage isn't ready");
            }
            _label.Visible = value;
        }
    }
    public string Text
    {
        get => _label?.Text;
        set
        {
            if (_label == null)
            {
                throw new Exception("WaveMessage isn't ready");
            }
            _label.Text = value;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        _label = GetNode<Label>("Label");
        _pressSpaceLabel = GetNode<Label>("PressSpace");
    }

    public void _on_arena_arena_state_changed(ArenaState state)
    {
        string text = state.State switch
        {
            ArenaStateEnum.Setup => "Arena opened",
            ArenaStateEnum.WaveChange => $"Wave {state.WaveNumber}",
            ArenaStateEnum.Cleared => "Arena Cleared!",
            _ => string.Empty
        };
        _awaitConfirmation = state.State != ArenaStateEnum.WaveChange;
        _pressSpaceLabel.Visible = _awaitConfirmation;
        Visible = true;
        if (_label != null)
        {
            Text = text;
        }
    }

    public override void _Process(double delta)
    {
        if (Visible && !_awaitConfirmation)
        {
            _timer -= delta;
            if (_timer <= 0 && Visible)
            {
                Visible = false;
                _timer = 4;
            }   
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept") && Visible && _awaitConfirmation)
        {
            Visible = false;
            EmitSignal(SignalName.MessagePressed);
        }
    }
}
