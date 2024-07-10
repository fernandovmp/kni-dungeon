using Dungeon.world.characters;
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
        _characterNode.Move(direction);
    }

    private Vector2 GetDirectionFromInput() => Input.GetVector("player_movement_left", "player_movement_right",
        "player_movement_up", "player_movement_down");
}