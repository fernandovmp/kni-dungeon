using Godot;

namespace Dungeon.world.characters;

public class Movement
{
    private AnimatedSprite2D _animation = default!;
    private CharacterBody2D _body = default!;

    public Movement(Node2D node)
    {
        _animation = node.GetNode<AnimatedSprite2D>("Body/Animation");
        _body = node.GetNode<CharacterBody2D>("Body");
    }

    public bool IsLookingLeft => _animation.FlipH;
    
    public void Move(Vector2 direction, double speed)
    {
        if (direction == Vector2.Zero)
        {
            Stop();
            return;
        }

        FlipSprite(direction);
        _body.Velocity = direction * (float)speed;
        _body.MoveAndSlide();
        _animation.Play("run");
    }

    private void FlipSprite(Vector2 direction)
    {
        if (direction.X > 0 && _animation.FlipH)
        {
            _animation.FlipH = false;
        }
        else if (direction.X < 0 && !_animation.FlipH)
        {
            _animation.FlipH = true;
        }
    }

    public void Stop()
    {
        _body.Velocity = Vector2.Zero;
        _animation.Play("idle");
    }
}