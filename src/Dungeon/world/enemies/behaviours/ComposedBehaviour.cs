using Godot;
using Godot.Collections;

namespace Dungeon.world.enemies.behaviours;

[GlobalClass]
[Tool]
public partial class ComposedBehaviour : BehaviourBase
{
    [Export] public Array<Resource> BehavioursResources { get; set; } = new Array<Resource>();
    
    public override void OnPhysicsProcess(double delta, EnemyNode enemyNode)
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