using Dungeon.world.characters;
using Dungeon.world.enemies.behaviours;
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
        Character.Combatent.Died += Died;
    }

    private void Died()
    {
        EmitSignal(SignalName.OnDied);
        Character.Sprite.Hide();
    }

    public void DeathAnimationFinished()
    {
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Behaviour != null)
        {
            Behaviour.OnPhysicsProcess(delta, this);
        }
    }
}