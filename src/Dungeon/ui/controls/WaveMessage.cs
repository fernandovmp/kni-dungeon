using System;
using Dungeon.world.arena;
using Godot;

namespace Dungeon.ui.controls;

public partial class WaveMessage : Panel
{
    private Label _label;

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
        if (_label != null)
        {
            Text = text;
        }
    }

}