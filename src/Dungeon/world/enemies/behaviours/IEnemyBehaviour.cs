namespace Dungeon.world.enemies.behaviours;

public interface IEnemyBehaviour
{
    void OnPhysicsProcess(double delta, EnemyNode enemyNode);
}