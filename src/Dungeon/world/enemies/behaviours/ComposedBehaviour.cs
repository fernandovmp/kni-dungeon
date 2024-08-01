using Godot;
using Godot.Collections;

namespace Dungeon.world.enemies.behaviours;

public partial class ComposedBehaviour : Resource, IEnemyBehaviour
{
    [Export] public Array<Resource> BehavioursResources { get; set; } = new Array<Resource>();
    
    public void OnPhysicsProcess(double delta, EnemyNode enemyNode)
    {
        foreach (var resource in BehavioursResources)
        {
            if (resource is IEnemyBehaviour behaviour)
            {
                behaviour.OnPhysicsProcess(delta, enemyNode);
            }
        }
    }
}