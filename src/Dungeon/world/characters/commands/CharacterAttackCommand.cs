using Dungeon.abstractions;

namespace Dungeon.world.characters.commands;

public struct CharacterAttackCommand : ICommand<CharacterNode>
{
    public void Execute(CharacterNode target)
    {
        target.Weapon.Attack(target.Body.Movement.IsLookingLeft);
    }
}