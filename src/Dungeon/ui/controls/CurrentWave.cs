using Dungeon.world.waves;
using Godot;

namespace Dungeon.ui.controls;

public partial class CurrentWave : Panel
{
    private Label _label;

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
    }

    public void OnWaveChange(WaveControllerNode.WaveChangedEvent @event)
    {
        _label.Text = $"Wave: {@event.WaveNumber}";
    }
}