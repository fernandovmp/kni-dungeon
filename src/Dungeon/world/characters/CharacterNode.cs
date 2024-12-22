using Dungeon.abstractions;
using Dungeon.world.characters.components;
using Dungeon.world.constants;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters;

public partial class CharacterNode : Node2D
{
    private AnimatedCharacterNode _sprite;
    [Export] public double Speed { get; set; } = 4;
    public CharacterResource Character { get; set; }
    [Export] public bool IsEnemy { get; set; }
    
    public CharacterBodyNode Body { get; private set;  }
    public WeaponNode Weapon { get; private set; }
    public CharacterState State { get; set; }
    public AnimatedCharacterNode Sprite => _sprite;

    public override void _Ready()
    {
        Weapon = GetNode<WeaponNode>("Body/Weapon");
        Body = GetNode<CharacterBodyNode>("Body");
        State = CharacterState.Idle;
    }

    public void Configure(CharacterResource character)
    {
        Character = character;
        Speed = character.Speed;
        _sprite = GetNode<AnimatedCharacterNode>("Body/Animation");
        _sprite.CharacterOwner = this;
        _sprite.SpriteFrames = Character.Sprite;
        Weapon.Configure(IsEnemy, Body);
        
        uint layer = PhysicsConstants.PlayerLayer;
        if (IsEnemy)
        {
            layer = PhysicsConstants.EnemyLayer;
        }
        Body.CollisionLayer = layer;
    }

    public void Execute(ICommand<CharacterNode> command)
    {
        if (command.CanExecute(this))
        {
            command.Execute(this);
        }
    }

    private void Hitted()
    {
        _sprite.RequestHit();
    }
}