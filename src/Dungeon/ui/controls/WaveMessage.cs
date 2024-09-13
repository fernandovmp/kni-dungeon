using System;
using Dungeon.world.arena;
using Godot;

namespace Dungeon.ui.controls;

public partial class WaveMessage : Panel
{
    private Label _label;
    private ArenaStateEnum _currentState;
    private double _timer = 4;
    
    [Signal]
    public delegate void WaveMessagePressedEventHandler(WaveMessagePressedEvent @event);

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
    }

    public void _on_arena_arena_state_changed(ArenaState state)
    {
        string text = state.State switch
        {
            ArenaStateEnum.Setup => "Arena opened",
            ArenaStateEnum.WaveChange => $"Wave {state.Index + 1}",
            ArenaStateEnum.Cleared => "Arena Cleared!",
            _ => string.Empty
        };
        _currentState = state.State;
        Visible = true;
        if (_label != null)
        {
            Text = text;
        }
    }

    public override void _Process(double delta)
    {
        if (Visible && _currentState == ArenaStateEnum.WaveChange)
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
        if ((_currentState == ArenaStateEnum.Setup || _currentState == ArenaStateEnum.Cleared) && @event.IsActionPressed("ui_accept") && Visible)
        {
            Visible = false;
            EmitSignal(SignalName.WaveMessagePressed, new WaveMessagePressedEvent(_currentState));
        }
    }
}

public partial class WaveMessagePressedEvent(ArenaStateEnum arenaState) : GodotObject
{
    public ArenaStateEnum ArenaState { get; } = arenaState;
}