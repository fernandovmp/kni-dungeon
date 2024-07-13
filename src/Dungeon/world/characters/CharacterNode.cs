using Dungeon.world.constants;
using Godot;

namespace Dungeon.world.characters;

public partial class CharacterNode : Node2D
{
    private WeaponNode _weapon = default!;
    private double _commandTime = 3;
    private int _command = -1;
    
    [Export] private double _speed = 4;
    [Export] public CharacterResource Character { get; set; }
    [Export] public bool IsEnemy { get; set; }
    
    public CharacterBodyNode Body { get; private set;  }

    public override void _Ready()
    {
        _weapon = GetNode<WeaponNode>("Body/Weapon");
        var sprite = GetNode<AnimatedSprite2D>("Body/Animation");
        sprite.SpriteFrames = Character.Sprite;
        Body = GetNode<CharacterBodyNode>("Body");
        _weapon.Configure(IsEnemy);
        Configure();
    }

    private void Configure()
    {
        uint layer = PhysicsConstants.PlayerLayer;
        if (IsEnemy)
        {
            layer = PhysicsConstants.EnemyLayer;
        }
        Body.CollisionLayer = layer;
    }

    public void Move(Vector2 direction)
    {
        Body.Movement.Move(direction, _speed);
    }

    public void Attack()
    {
        _weapon.Attack(Body.Movement.IsLookingLeft);
    }
}