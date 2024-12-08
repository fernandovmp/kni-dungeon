using Godot;

namespace Dungeon.world.particles;

public partial class EnemyExplosionNode : CpuParticles2D
{
    public override void _Ready()
    {
        base._Ready();
        Emitting = true;
        Connect(CpuParticles2D.SignalName.Finished, new Callable(this, nameof(OnFinished)));
    }

    public void OnFinished()
    {
        QueueFree();
    }
}