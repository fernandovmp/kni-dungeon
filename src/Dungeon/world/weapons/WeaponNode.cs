using Godot;

namespace Dungeon.world.characters;

public partial class WeaponNode : Node2D
{
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;
    private WeaponBodyNode _weaponBody;
    private bool _isLookingLeft = false;
    [Export] public AudioStream CriticalSound;
    [Export] public AudioStream HitSound;
    public AudioStreamPlayer2D AttackSound { get; private set; }
    
    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite");
        _animationPlayer = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
        _animationPlayer.Connect("animation_finished", new Callable(this, nameof(ResetAnimation)));
        _weaponBody = GetNode<WeaponBodyNode>("Sprite/Body");
        AttackSound = GetNode<AudioStreamPlayer2D>("AttackSound");
    }

    public void Attack(bool isDirectionLeft)
    {
        _weaponBody.Enable();
        if (_isLookingLeft != isDirectionLeft)
        {
            _isLookingLeft = isDirectionLeft;
            Scale = new Vector2(Scale.X * -1, Scale.Y);
        }
        _sprite.Visible = true;
        _animationPlayer.Play("attack");
        PlayAttackSound();
    }

    private void PlayAttackSound()
    {
        AttackSound.Play();
    }

    public void ResetAnimation(string name)
    {
        _weaponBody.Disable();
        _sprite.Visible = false;
    }

    public void Configure(bool isEnemy, CharacterBodyNode character)
    {
        _weaponBody.Configure(isEnemy, character);
    }
}