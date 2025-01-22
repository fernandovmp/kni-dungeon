using Godot;

namespace Dungeon.ui.controls;

public partial class VolumeSlider : HSlider
{
    private int _masterBus;
    public const string Master = "Master";
    public override void _Ready()
    {
        base._Ready();
        MaxValue = 1;
        MinValue = 0;
        Step = 0.05;
        _masterBus = AudioServer.GetBusIndex(Master);
        Value = 0.5;

    }

    public override void _ValueChanged(double newValue)
    {
        base._ValueChanged(newValue);
        AudioServer.SetBusVolumeDb(_masterBus, Mathf.LinearToDb((float)newValue));
            
    }
}