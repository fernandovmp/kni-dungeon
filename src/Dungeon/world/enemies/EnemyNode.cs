using System.Collections.Generic;
using Dungeon.services.context_map;
using Dungeon.world.characters;
using Dungeon.world.characters.components;
using Dungeon.world.enemies.behaviours;
using Dungeon.world.player;
using Dungeon.world.weapons;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.enemies;

[Tool]
public partial class EnemyNode : Node2D
{
    public CharacterBodyNode Character { get; set; }
    public ContextMapNode ContextMap { get; private set; }
    public BehaviourBase Behaviour { get; set; }
    public Dictionary<string, object> BehaviorData { get; private set; } = new Dictionary<string, object>(); 
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

    private CharacterResource _characterResource;
    [Export] public CharacterResource CharacterResource { get => _characterResource;
        set
        {
            _characterResource = value;
            if (Engine.IsEditorHint() && _characterResource != null)
            {
                GetNode<AnimatedSprite2D>("Body/Animation").SpriteFrames = _characterResource.Sprite;
            }
        }
        
    }
    
    [Export]
    public PackedScene LifeDropScene { get; set; }
    
    [Signal]
    public delegate void OnDiedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        if (Engine.IsEditorHint())
        {
            return;
        }
        Character = GetNode<CharacterBodyNode>("Body");
        Character.IsEnemy = true;
        Character.Configure(CharacterResource);
        var combatent = Character.GetMetadata<CombatentNode>(nameof(CombatentNode));
        combatent.Connect(CombatentNode.SignalName.Died, new Callable(this, nameof(Died)));
        ContextMap = GetNode<ContextMapNode>("Body/ContextRaycast");
    }

    private void Died()
    {
        Character.State = CharacterState.Dead;
        Character.Sprite.Hide();
        var weapon = Character.GetMetadata<WeaponNode>(nameof(WeaponNode));
        weapon?.Hide();
        EmitSignal(SignalName.OnDied);

        if (ShouldDropLife())
        {
            var rootNode = GetTree().GetFirstNodeInGroup("ItemsRoot");
            if (rootNode != null)
            {
                var life = LifeDropScene.Instantiate<Node2D>();
                life.GlobalPosition = Character.GlobalPosition;
                rootNode.CallDeferred("add_child", life);
            }
        }
    }

    public bool ShouldDropLife()
    {
        var playerNode = GetTree().GetFirstNodeInGroup("Player");
        const int lifeLimit = 11;
        if (playerNode is PlayerNode player)
        {
            CombatentNode playerCombatent = player.Character.GetMetadata<CombatentNode>(nameof(CombatentNode));
            int currentLife = playerCombatent.Life;
            
            float chance = CharacterResource.LifeDropChance * ((lifeLimit - currentLife) * 0.1f);
            float rand = GD.Randf();
            return rand <= chance;
        }

        return false;
    }

    public void DeathAnimationFinished()
    {
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint())
        {
            return;
        }
        base._PhysicsProcess(delta);
        Behaviour?.OnPhysicsProcess(delta, this);
    }
}