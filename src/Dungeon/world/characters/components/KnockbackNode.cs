using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters.components;

public partial class KnockbackNode : Node
{
    [Export] public PhysicsBody2D Target { get; set; }
    
    private KnockbackData _knockback;

    public override void _EnterTree()
    {
        base._EnterTree();
        Owner.SetMetadata(nameof(KnockbackNode), this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Owner.RemoveMeta(nameof(KnockbackNode));
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (_knockback.Force > 0)
        {
            var motion = _knockback.Direction * (float)(_knockback.Force * delta);
            _knockback.Force = Mathf.Lerp(_knockback.Force, 0, 0.1);
            Target.MoveAndCollide(motion);
        }
    }
    
    public void Apply(KnockbackData knockback)
    {
        _knockback = knockback;
    }
}

public struct KnockbackData(Vector2 direction, double force)
{
    public Vector2 Direction = direction;
    public double Force = force;
}
