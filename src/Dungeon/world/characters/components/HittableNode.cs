using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters.components;

public partial class HittableNode : Node
{
    [Export] public bool IsInvencible { get; set; }
    [Export] public double InvencibilityTime { get; set; } = 0.6;
    [Export] public CollisionObject2D Hitbox { get; set; }
    
    [Signal] public delegate void HittedEventHandler(CollisionObject2D body);

    public override void _EnterTree()
    {
        base._EnterTree();
        Owner.SetMetadata(nameof(HittableNode), this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Owner.RemoveMeta(nameof(HittableNode));
    }

    public void Hit(CollisionObject2D body)
    {
        if (!IsInvencible)
        {
            EmitSignal(SignalName.Hitted, body);
            SetInvecibilityAsync();
        }
    }
    
    public async void SetInvecibilityAsync()
    {
        if (IsInvencible) return;
        if (InvencibilityTime == 0) return;
        
        IsInvencible = true;
        await this.WaitForSeconds(InvencibilityTime);
        IsInvencible = false;
    }
}