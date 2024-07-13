using Godot;

namespace Dungeon.world.characters;

public partial class WeaponNode : Node2D
{
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;
    private WeaponBodyNode _weaponBody;
    private bool _isLookingLeft = false;
    
    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite");
        _animationPlayer = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
        _animationPlayer.Connect("animation_finished", new Callable(this, nameof(ResetAnimation)));
        _weaponBody = GetNode<WeaponBodyNode>("Sprite/Body");
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
    }

    public void ResetAnimation(string name)
    {
        _weaponBody.Disable();
        _sprite.Visible = false;
    }

    public void Configure(bool isEnemy)
    {
        _weaponBody.ConfigureLayers(isEnemy);
    }
}