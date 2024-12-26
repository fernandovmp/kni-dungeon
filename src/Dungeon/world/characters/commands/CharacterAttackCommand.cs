using Dungeon.abstractions;
using FernandoVmp.GodotUtils.Extensions;

namespace Dungeon.world.characters.commands;

public struct CharacterAttackCommand : ICommand<CharacterBodyNode>
{
    public bool CanExecute(CharacterBodyNode target) => target.State is CharacterState.Idle or CharacterState.Moving && target.HasMeta(nameof(WeaponNode));

    public void Execute(CharacterBodyNode target)
    {
        var weapon = target.GetMetadata<WeaponNode>(nameof(WeaponNode));
        weapon.Attack(target.Movement.IsLookingLeft);
    }
}