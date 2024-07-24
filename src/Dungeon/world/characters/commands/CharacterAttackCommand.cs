using Dungeon.abstractions;

namespace Dungeon.world.characters.commands;

public struct CharacterAttackCommand : ICommand<CharacterNode>
{
    public bool CanExecute(CharacterNode target) => target.State is CharacterState.Idle or CharacterState.Moving;

    public void Execute(CharacterNode target)
    {
        target.Weapon.Attack(target.Body.Movement.IsLookingLeft);
    }
}