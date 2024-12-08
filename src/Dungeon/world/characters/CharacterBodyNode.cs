using Godot;

namespace Dungeon.world.characters;

public partial class CharacterBodyNode : CharacterBody2D
{
    private KnockbackData _knockback;
    public Movement Movement { get; private set; }
    public bool IsInvencible { get; private set; }
    public CharacterNode CharacterOwner { get; private set; }
    public NavigationAgent2D NavigationAgent { get; private set; }
    public AudioStreamPlayer2D HitSoundPlayer { get; private set; }
    
    public override void _Ready()
    {
        Movement = new Movement(this);
        CharacterOwner = GetNode<CharacterNode>("..");
        NavigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        HitSoundPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
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