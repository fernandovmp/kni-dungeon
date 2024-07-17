using Dungeon.world.characters;
using Dungeon.world.enemies.behaviours;
using Godot;

namespace Dungeon.world.enemies;

public partial class EnemyNode : Node2D
{
    public CharacterNode Character { get; set; }
    public IEnemyBehaviour Behaviour { get; set; }
    private Resource _behaviourResource;
    [Export]
    public Resource BehaviourResource { get => _behaviourResource;
        set
        {
            _behaviourResource = value;
            if (_behaviourResource is IEnemyBehaviour enemyBehaviour)
            {
                Behaviour = enemyBehaviour;
            }
            else
            {
                Behaviour = null;
            }
        } }

    public override void _Ready()
    {
        base._Ready();
        Character = GetNode<CharacterNode>("Character");
        Character.IsEnemy = true;
        Character.Configure();
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