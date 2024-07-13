using Dungeon.world.characters;
using Dungeon.world.characters.commands;
using Godot;

namespace Dungeon.world.player;

public partial class PlayerNode : Node2D
{
    private CharacterNode _characterNode = default!;
    
    public override void _Ready()
    {
        _characterNode = GetNode<CharacterNode>("Character");
    }

    public override void _PhysicsProcess(double detla)
    {
        Vector2 direction = GetDirectionFromInput();
        var command = new CharacterMovementCommand(direction);
        command.Execute(_characterNode);
    }

    private Vector2 GetDirectionFromInput() => Input.GetVector("player_movement_left", "player_movement_right",
        "player_movement_up", "player_movement_down");

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            var command = new CharacterAttackCommand();
            command.Execute(_characterNode);
        }
    }
}