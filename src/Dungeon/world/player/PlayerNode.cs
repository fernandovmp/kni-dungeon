using Dungeon.ui.health;
using Dungeon.world.characters;
using Dungeon.world.characters.commands;
using Godot;

namespace Dungeon.world.player;

public partial class PlayerNode : Node2D
{
    private CharacterNode _characterNode = default!;
    private HeartGaugeNode _healthGauge;
    [Export] public CharacterResource CharacterResource { get; set; }
    
    public override void _Ready()
    {
        _characterNode = GetNode<CharacterNode>("Character");
        _characterNode.Configure(CharacterResource);
        _healthGauge = GetNode<HeartGaugeNode>("../CanvasLayer/HeartGauge");
        _healthGauge.Count = CharacterResource.Life / 2;
        _healthGauge.Health = CharacterResource.Life;
        _characterNode.Combatent.Hitted += UpdateHealth;
        _characterNode.Combatent.Died += UpdateHealth;
    }

    private void UpdateHealth()
    {
        _healthGauge.Health = _characterNode.Combatent.Life;
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