using Godot;

namespace Dungeon.world.particles;

public partial class EnemyExplosionNode : CpuParticles2D
{
    private AudioStreamPlayer _audio;

    public override void _Ready()
    {
        base._Ready();
        _audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    }

    public void Play()
    {
        Emitting = true;
        _audio.Play();
    }
}