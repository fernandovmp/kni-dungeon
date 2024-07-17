using Dungeon.abstractions;
using Godot;

namespace Dungeon.world.characters.commands;

public class CharacterReceivedAttackCommand(float rotationDegrees, Node2D body) : ICommand<CharacterNode>
{
    public void Execute(CharacterNode target)
    {
        if(target.Body.IsInvencible) return;
        target.Body.SetInvecibilityAsync();
        int force = GetForce();
        Vector2 direction = GetDirection(target);
        target.Body.ApplyKnockBack(force, direction);
    }

    private Vector2 GetDirection(CharacterNode target) => body.GlobalPosition.DirectionTo(target.Body.GlobalPosition);

    private int GetForce()
    {
        if (rotationDegrees >= 45 && rotationDegrees <= 120)
        {
            return 240;
        }
        return 80;
    }
}