using Dungeon.world.characters;
using Dungeon.world.characters.components;
using Dungeon.world.enemies.behaviours;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.enemies;

public partial class EnemyNode : Node2D
{
    public CharacterNode Character { get; set; }
    public BehaviourBase Behaviour { get; set; }
    private BehaviourBase _behaviourResource;
    [Export]
    public BehaviourBase BehaviourResource { get => _behaviourResource;
        set
        {
            _behaviourResource = value;
            if (_behaviourResource is { } enemyBehaviour)
            {
                Behaviour = enemyBehaviour;
            }
            else
            {
                Behaviour = null;
            }
        } }
    [Export] public CharacterResource CharacterResource { get; set; }
    
    [Signal]
    public delegate void OnDiedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        Character = GetNode<CharacterNode>("Character");
        Character.IsEnemy = true;
        Character.Configure(CharacterResource);
        var combatent = Character.Body.GetMetadata<CombatentNode>(nameof(CombatentNode));
        combatent.Connect(CombatentNode.SignalName.Died, new Callable(this, nameof(Died)));
    }

    private void Died()
    {
        EmitSignal(SignalName.OnDied);
        Character.State = CharacterState.Dead;
        Character.Body.Sprite.Hide();
    }

    public void DeathAnimationFinished()
    {
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Behaviour?.OnPhysicsProcess(delta, this);
    }
}