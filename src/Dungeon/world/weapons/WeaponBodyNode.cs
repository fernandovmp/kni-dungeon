using Dungeon.world.characters.commands;
using Dungeon.world.constants;
using Godot;

namespace Dungeon.world.characters;

public partial class WeaponBodyNode : Area2D
{

    private CollisionShape2D _shape;
    private Node2D _sprite;

    public override void _Ready()
    {
        base._Ready();
        _shape = GetNode<CollisionShape2D>("CollisionShape2D");
        _sprite = GetNode<Node2D>(".."); 
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
    }

    public void ConfigureLayers(bool isEnemy)
    {
        uint mask = PhysicsConstants.EnemyLayer;
        if (isEnemy)
        {
            mask = PhysicsConstants.PlayerLayer;
        }
        CollisionMask = mask;
    }

    public void Enable() => _shape.Disabled = false;
    public void Disable() => _shape.Disabled = true;

    private void OnBodyEntered(Node2D body)
    { 
        if (body is CharacterBodyNode characterBodyNode)
        {
            var rotationDegrees = Mathf.RadToDeg(_sprite.Rotation);
            var command = new CharacterReceivedAttackCommand(rotationDegrees);
            command.Execute(characterBodyNode.CharacterOwner);
        }
    }
}