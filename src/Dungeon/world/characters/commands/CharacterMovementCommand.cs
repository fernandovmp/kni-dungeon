using Dungeon.abstractions;
using Godot;

namespace Dungeon.world.characters.commands;

public struct CharacterMovementCommand(Vector2 direction) : ICommand<CharacterBodyNode>
{
    public bool CanExecute(CharacterBodyNode target) => target.State is CharacterState.Idle or CharacterState.Moving;

    public void Execute(CharacterBodyNode target)
    {
        target.Movement.Move(direction, target.Speed);
        //
        // float speed = (float) target.Speed;
        // var steering = (direction * speed) - target.Velocity;
        // target.Movement.Move(steering * delta, target.Speed);
    }
}