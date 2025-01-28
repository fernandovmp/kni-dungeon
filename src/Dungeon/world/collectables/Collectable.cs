using Godot;

namespace Dungeon.world.collectables;

public abstract partial class Collectable : Node2D
{
    [Export] public Area2D Collision { get; set; }
    
    [Signal]
    public delegate void CollectedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        Collision?.Connect(Area2D.SignalName.BodyEntered, new Callable(this,nameof(OnCollected)));
    }

    public abstract void OnCollected(Node2D collector);
}