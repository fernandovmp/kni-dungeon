using Dungeon.abstractions;
using Godot;

namespace Dungeon.world.characters.commands;

public struct CharacterMovementCommand(Vector2 Direction) : ICommand<CharacterNode>
{
    public bool CanExecute(CharacterNode target) => target.State is CharacterState.Idle or CharacterState.Moving;

    public void Execute(CharacterNode target)
    {
        target.Body.Movement.Move(Direction, target.Speed);
    }
}