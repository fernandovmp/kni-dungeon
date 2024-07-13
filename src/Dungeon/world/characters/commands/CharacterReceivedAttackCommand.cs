using Dungeon.abstractions;
using Godot;

namespace Dungeon.world.characters.commands;

public class CharacterReceivedAttackCommand(float RotationDegrees) : ICommand<CharacterNode>
{
    public void Execute(CharacterNode target)
    {
        if(target.Body.IsInvencible) return;
        target.Body.SetInvecibilityAsync();
        int force = GetForce();
        target.Body.ApplyKnockBack(force, Vector2.Zero);
    }

    private int GetForce()
    {
        if (RotationDegrees >= 45 && RotationDegrees <= 120)
        {
            return 60;
        }
        return 20;
    }
}