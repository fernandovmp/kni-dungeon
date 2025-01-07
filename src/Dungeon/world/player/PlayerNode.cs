using Dungeon.abstractions;
using Dungeon.world.characters;
using Dungeon.world.characters.commands;
using Dungeon.world.characters.components;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.player;

public partial class PlayerNode : Node2D
{
    [Export] public CharacterResource CharacterResource { get; set; }
    public CharacterBodyNode Character { get; private set; }
    
    [Signal]
    public delegate void PlayerReadiedEventHandler(PlayerNode player);
    [Signal]
    public delegate void CombatentUpdatedEventHandler(PlayerNode player);
    [Signal]
    public delegate void PlayerDiedEventHandler(PlayerNode player);
    
    public override void _Ready()
    {
        Character = GetNode<CharacterBodyNode>("Body");
        Character.Configure(CharacterResource);
        EmitSignal(SignalName.PlayerReadied, this);
    }

    public void OnDied()
    {
        EmitDiedEvent();
    }

    private void EmitDiedEvent()
    {
        EmitCombatentUpdate();
        EmitSignal(SignalName.PlayerDied, this);
    }

    private void EmitCombatentUpdate() => EmitSignal(SignalName.CombatentUpdated, this);

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = GetDirectionFromInput();
        Character.TryExecute(new CharacterMovementCommand(direction));
    }

    private Vector2 GetDirectionFromInput() => Input.GetVector("player_movement_left", "player_movement_right",
        "player_movement_up", "player_movement_down");

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            Character.TryExecute(new CharacterAttackCommand());
        }
    }
}