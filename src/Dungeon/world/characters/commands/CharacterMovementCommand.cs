using Dungeon.abstractions;
using Godot;

namespace Dungeon.world.characters.commands;

public struct CharacterMovementCommand(Vector2 direction) : ICommand<CharacterBodyNode>
{
    public bool CanExecute(CharacterBodyNode target) => target.State is CharacterState.Idle or CharacterState.Moving;

    public void Execute(CharacterBodyNode target)
    {
        target.Movement.Move(direction, target.Speed);
    }
}

public struct CharacterMovementWithSteeringCommand(Vector2 direction, double delta) : ICommand<CharacterBodyNode>
{
    public bool CanExecute(CharacterBodyNode target) => target.State is CharacterState.Idle or CharacterState.Moving;

    public void Execute(CharacterBodyNode target)
    {
        float speed = (float) target.Speed;
        var steering = (direction * speed) - target.Velocity;
        steering = steering * (float) delta;
        var velocity = target.Velocity + steering;
        target.Movement.Move(velocity.LimitLength(speed), 1);
    }
}