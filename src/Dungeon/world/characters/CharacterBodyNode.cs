using Dungeon.abstractions;
using Dungeon.world.characters.commands;
using Dungeon.world.characters.components;
using Dungeon.world.constants;
using Dungeon.world.weapons;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters;

public partial class CharacterBodyNode : CharacterBody2D
{
    public CharacterResource Character { get; set; }
    public CharacterState State { get; set; } = CharacterState.None;
    [Export] public double Speed { get; set; } = 4;
    public Movement Movement { get; private set; }
    public AudioStreamPlayer2D HitSoundPlayer { get; private set; }
    public AnimatedCharacterNode Sprite { get; private set; }
    [Export] public bool IsEnemy { get; set; }
    
    
    [Signal]
    public delegate void CharacterDiedEventHandler();
    
    public override void _Ready()
    {
        HitSoundPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        Sprite = GetNode<AnimatedCharacterNode>("Animation");
        Sprite.CharacterOwner = this;
    }

    public void Configure(CharacterResource character)
    {
        Character = character;
        Speed = character.Speed;
        Sprite.SpriteFrames = character.Sprite;
        Movement = new Movement(this);
        uint layer = PhysicsConstants.PlayerLayer;
        
        var combatent = this.GetMetadata<CombatentNode>(nameof(CombatentNode));
        combatent.Load(character);
        
        if (IsEnemy)
        {
            layer = PhysicsConstants.EnemyLayer;
        }
        CollisionLayer = layer;
        State = CharacterState.Idle;
        AddWeapon();
    }

    private void AddWeapon()
    {
        if (HasMeta(nameof(WeaponNode)) || Character.Weapon == null)
            return;

        var weapon = Character.Weapon.Instantiate<WeaponNode>();
        AddChild(weapon);
        weapon.UpdateOwner(this);
    }

    private void OnDied()
    {
        State = CharacterState.Dead;
        EmitSignal(SignalName.CharacterDied);
    }

    private void OnHit(CollisionObject2D body)
    {
        if (body is WeaponBodyNode weaponBody)
        {
            var command = new CharacterReceivedAttackCommand(weaponBody, weaponBody.CharacterOwner);
            this.TryExecute(command);
        }
    }
}