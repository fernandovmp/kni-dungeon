using Godot;

namespace Dungeon.world.particles;

public partial class EnemyExplosionNode : CpuParticles2D
{
    public void Play()
    {
        Emitting = true;
    }
}