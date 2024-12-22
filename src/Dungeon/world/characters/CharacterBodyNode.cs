using Dungeon.world.characters.components;
using Dungeon.world.constants;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters;

public partial class CharacterBodyNode : CharacterBody2D
{
    public CharacterResource Character { get; set; }
    public CharacterState State { get; set; }
    [Export] public double Speed { get; set; } = 4;
    
    private KnockbackData _knockback;
    public Movement Movement { get; private set; }
    public bool IsInvencible { get; private set; }
    public CharacterNode CharacterOwner { get; private set; }
    public AudioStreamPlayer2D HitSoundPlayer { get; private set; }
    public AnimatedCharacterNode Sprite { get; private set; }
    
    
    [Signal]
    public delegate void CharacterDiedEventHandler();
    
    public override void _Ready()
    {
        Movement = new Movement(this);
        CharacterOwner = GetNode<CharacterNode>("..");
        HitSoundPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        Sprite = GetNode<AnimatedCharacterNode>("Animation");
        Sprite.CharacterOwner = this;
    }

    public void Configure(CharacterResource character, bool isEnemy)
    {
        Character = character;
        Speed = character.Speed;
        Sprite.SpriteFrames = character.Sprite;
        uint layer = PhysicsConstants.PlayerLayer;
        
        var combatent = this.GetMetadata<CombatentNode>(nameof(CombatentNode));
        combatent.Load(character);
        
        if (isEnemy)
        {
            layer = PhysicsConstants.EnemyLayer;
        }
        CollisionLayer = layer;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (_knockback.force > 0)
        {
            var motion = _knockback.direction * (float)(_knockback.force * delta);
            _knockback.force = Mathf.Lerp(_knockback.force, 0, 0.1);
            MoveAndCollide(motion);
        }
    }

    private void OnDied()
    {
        State = CharacterState.Dead;
        EmitSignal(SignalName.CharacterDied);
    }

    public async void SetInvecibilityAsync()
    {
        if(IsInvencible) return;
        IsInvencible = true;
        await ToSignal(GetTree().CreateTimer(0.6), "timeout");
        IsInvencible = false;
    }

    public void ApplyKnockBack(int force, Vector2 direction)
    {
        _knockback = new KnockbackData(direction, force);
    }
    
    private struct KnockbackData(Vector2 direction, double force)
    {
        public Vector2 direction = direction;
        public double force = force;
    }
}