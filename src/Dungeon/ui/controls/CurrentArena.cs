using Dungeon.world.arena;
using Godot;

namespace Dungeon.ui.controls;

public partial class CurrentArena : Panel
{
    private Label _label;

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
    }

    public void OnWaveChange(ArenaState arenaState)
    {
        bool waveChanged = arenaState.State == ArenaStateEnum.WaveChange; 
        if (waveChanged)
        {
            _label.Text = $"Arena: {arenaState.Index + 1}";
        }

        Visible = waveChanged;
    }
}