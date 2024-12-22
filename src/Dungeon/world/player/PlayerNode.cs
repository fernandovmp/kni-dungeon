using Dungeon.world.characters;
using Dungeon.world.characters.commands;
using Dungeon.world.characters.components;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.player;

public partial class PlayerNode : Node2D
{
    private CharacterNode _characterNode = default!;
    [Export] public CharacterResource CharacterResource { get; set; }
    public CharacterNode Character => _characterNode;
    
    [Signal]
    public delegate void PlayerReadiedEventHandler(PlayerNode player);
    [Signal]
    public delegate void CombatentUpdatedEventHandler(PlayerNode player);
    [Signal]
    public delegate void PlayerDiedEventHandler(PlayerNode player);
    
    public override void _Ready()
    {
        _characterNode = GetNode<CharacterNode>("Character");
        _characterNode.Configure(CharacterResource);
        EmitSignal(SignalName.PlayerReadied, this);
    }

    public void OnDied()
    {
        _characterNode.Sprite.RequestDeath();
        EmitDiedEvent();
    }

    private void EmitDiedEvent()
    {
        EmitCombatentUpdate();
        EmitSignal(SignalName.PlayerDied, this);
    }

    private void EmitCombatentUpdate() => EmitSignal(SignalName.CombatentUpdated, this);

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