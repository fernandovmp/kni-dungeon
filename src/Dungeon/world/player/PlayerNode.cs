using Dungeon.world.characters;
using Dungeon.world.characters.commands;
using Godot;

namespace Dungeon.world.player;

public partial class PlayerNode : Node2D
{
    private CharacterNode _characterNode = default!;
    [Export] public CharacterResource CharacterResource { get; set; }
    
    public override void _Ready()
    {
        _characterNode = GetNode<CharacterNode>("Character");
        _characterNode.Configure(CharacterResource);
    }

    public override void _PhysicsProcess(double detla)
    {
        Vector2 direction = GetDirectionFromInput();
        _characterNode.Execute(new CharacterMovementCommand(direction));
    }

    private Vector2 GetDirectionFromInput() => Input.GetVector("player_movement_left", "player_movement_right",
        "player_movement_up", "player_movement_down");

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            _characterNode.Execute(new CharacterAttackCommand());
        }
    }
}