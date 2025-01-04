using Dungeon.world.characters;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.weapons;

public partial class WeaponNode : Node2D
{
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;
    private WeaponBodyNode _weaponBody;
    private bool _isLookingLeft = false;
    [Export] public AudioStream CriticalSound;
    [Export] public AudioStream HitSound;
    public AudioStreamPlayer2D AttackSound { get; private set; }

    protected virtual string BodyPath => "Sprite/Body";
    
    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _weaponBody = GetNode<WeaponBodyNode>(BodyPath);
        AttackSound = GetNode<AudioStreamPlayer2D>("AttackSound");
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        SetMetadata();
    }

    public void UpdateOwner(Node owner)
    {
        Owner = owner;
        SetMetadata();
    }

    private void SetMetadata()
    {
        Owner?.SetMetadata(nameof(WeaponNode), this);
        if (Owner is CharacterBodyNode characterBodyNode)
        {
            Configure(characterBodyNode);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Owner.RemoveMeta(nameof(WeaponNode));
    }

    public virtual void Attack(bool isDirectionLeft)
    {
        if (_isLookingLeft != isDirectionLeft)
        {
            _isLookingLeft = isDirectionLeft;
            Scale = new Vector2(Scale.X * -1, Scale.Y);
        }
        _animationPlayer.Play("attack");
        PlayAttackSound();
    }

    private void PlayAttackSound()
    {
        AttackSound.Play();
    }
    
    public void Configure(CharacterBodyNode character)
    {
        _weaponBody.Configure(character.IsEnemy, character);
    }
}