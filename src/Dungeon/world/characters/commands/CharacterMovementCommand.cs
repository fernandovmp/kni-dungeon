using Dungeon.abstractions;
using Godot;

namespace Dungeon.world.characters.commands;

public struct CharacterMovementCommand(Vector2 Direction) : ICommand<CharacterBodyNode>
{
    public bool CanExecute(CharacterBodyNode target) => target.State is CharacterState.Idle or CharacterState.Moving;

    public void Execute(CharacterBodyNode target)
    {
        target.Movement.Move(Direction, target.Speed);
    }
}