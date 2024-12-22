using Godot;

namespace Dungeon.world.characters;

public class Movement
{
    private AnimatedCharacterNode _animation;
    private CharacterBodyNode _body;

    public Movement(Node2D node)
    {
        _body = (CharacterBodyNode) node;
        _animation = _body.Sprite;
        _animation.Play("idle");
    }

    public bool IsLookingLeft => _animation.FlipH;
    
    public void Move(Vector2 direction, double speed)
    {
        if (direction == Vector2.Zero)
        {
            Stop();
            return;
        }

        _body.State = CharacterState.Moving;
        _animation.FlipSprite(direction);
        _body.Velocity = direction * (float)speed;
        _body.MoveAndSlide();
        _animation.RequestRun();
    }

    public void Stop()
    {
        _body.Velocity = Vector2.Zero;
        _body.State = CharacterState.Idle;
        _animation.RequestIdle();
    }
}