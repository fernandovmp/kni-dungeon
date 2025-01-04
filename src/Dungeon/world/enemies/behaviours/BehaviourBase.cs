using Godot;

namespace Dungeon.world.enemies.behaviours;

[GlobalClass]
[Tool]
public abstract partial class BehaviourBase : Resource, IEnemyBehaviour
{
    public abstract void OnPhysicsProcess(double delta, EnemyNode enemyNode);
}