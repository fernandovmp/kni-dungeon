using Dungeon.abstractions;
using Godot;

namespace Dungeon.world.characters.commands;

public class CharacterReceivedAttackCommand(float rotationDegrees, CharacterNode body) : ICommand<CharacterNode>
{
    public bool CanExecute(CharacterNode target) => !target.Body.IsInvencible
                                                    && target.IsAlive;

    public void Execute(CharacterNode target)
    {
        target.Body.SetInvecibilityAsync();
        int force = GetForce() + body.Combatent.Force - target.Combatent.Resistance;
        Vector2 direction = GetDirection(target);
        target.Combatent.DealDamage();
        target.Body.ApplyKnockBack(force, direction);
    }

    private Vector2 GetDirection(CharacterNode target) => body.Body.GlobalPosition.DirectionTo(target.Body.GlobalPosition);

    private int GetForce()
    {
        if (rotationDegrees >= 45 && rotationDegrees <= 120)
        {
            return 230;
        }
        return 70;
    }
}