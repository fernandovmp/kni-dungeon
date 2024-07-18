using Dungeon.world.constants;
using Godot;

namespace Dungeon.world.characters;

public partial class CharacterNode : Node2D
{
    [Export] public double Speed { get; set; } = 4;
    public CharacterResource Character { get; set; }
    [Export] public bool IsEnemy { get; set; }
    
    public CharacterBodyNode Body { get; private set;  }
    public WeaponNode Weapon { get; private set; }
    public Combatent Combatent { get; private set; }
    

    public override void _Ready()
    {
        Weapon = GetNode<WeaponNode>("Body/Weapon");
        Body = GetNode<CharacterBodyNode>("Body");
    }

    public void Configure(CharacterResource character)
    {
        Character = character;
        Combatent = Combatent.From(Character);
        var sprite = GetNode<AnimatedSprite2D>("Body/Animation");
        sprite.SpriteFrames = Character.Sprite;
        Weapon.Configure(IsEnemy, Body);
        
        uint layer = PhysicsConstants.PlayerLayer;
        if (IsEnemy)
        {
            layer = PhysicsConstants.EnemyLayer;
        }
        Body.CollisionLayer = layer;
    }
}