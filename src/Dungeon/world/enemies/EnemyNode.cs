using Dungeon.world.characters;
using Dungeon.world.characters.components;
using Dungeon.world.enemies.behaviours;
using Dungeon.world.weapons;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.enemies;

public partial class EnemyNode : Node2D
{
    public CharacterBodyNode Character { get; set; }
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
        Character = GetNode<CharacterBodyNode>("Body");
        Character.IsEnemy = true;
        Character.Configure(CharacterResource);
        var combatent = Character.GetMetadata<CombatentNode>(nameof(CombatentNode));
        combatent.Connect(CombatentNode.SignalName.Died, new Callable(this, nameof(Died)));
    }

    private void Died()
    {
        Character.State = CharacterState.Dead;
        Character.Sprite.Hide();
        var weapon = Character.GetMetadata<WeaponNode>(nameof(WeaponNode));
        weapon?.Hide();
        EmitSignal(SignalName.OnDied);
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