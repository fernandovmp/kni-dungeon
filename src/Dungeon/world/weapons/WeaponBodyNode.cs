using Dungeon.world.constants;
using Godot;

namespace Dungeon.world.characters;

public partial class WeaponBodyNode : Area2D
{

    private CollisionShape2D _shape;
    
    public override void _Ready()
    {
        base._Ready();
        _shape = GetNode<CollisionShape2D>("CollisionShape2D");
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
    }

    public void ConfigureLayers(bool isEnemy)
    {
        uint mask = PhysicsConstants.PlayerLayer;
        if (isEnemy)
        {
            mask = PhysicsConstants.EnemyLayer;
        }
        CollisionMask = mask;
    }

    public void Enable() => _shape.Disabled = false;
    public void Disable() => _shape.Disabled = true;

    private void OnBodyEntered(Node2D body)
    {
        GD.Print(body.GetInstanceId() + " Entered");   
    }

    private void OnBodyExited(Node2D body)
    {
        GD.Print(body.GetInstanceId() + " Exited");
    }
}