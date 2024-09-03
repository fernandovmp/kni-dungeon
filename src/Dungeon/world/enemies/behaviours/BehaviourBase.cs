using Godot;

namespace Dungeon.world.enemies.behaviours;

[GlobalClass]
public abstract partial class BehaviourBase : Resource, IEnemyBehaviour
{
    public abstract void OnPhysicsProcess(double delta, EnemyNode enemyNode);
}