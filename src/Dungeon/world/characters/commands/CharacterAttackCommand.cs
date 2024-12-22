using Dungeon.abstractions;

namespace Dungeon.world.characters.commands;

public struct CharacterAttackCommand : ICommand<CharacterBodyNode>
{
    public bool CanExecute(CharacterBodyNode target) => target.State is CharacterState.Idle or CharacterState.Moving;

    public void Execute(CharacterBodyNode target)
    {
        target.CharacterOwner.Weapon.Attack(target.Movement.IsLookingLeft);
    }
}