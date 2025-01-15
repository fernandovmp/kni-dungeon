using Godot;

namespace Dungeon.services;

public partial class CameraFollow : Camera2D
{
    [Export]
    public string TargetGroup { get; set; }
    
    private Node2D _target;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (_target is null)
        {
            var node = GetTree().GetFirstNodeInGroup(TargetGroup);
            if (node is Node2D target)
            {
                _target = target;
            }
        }
        if (_target is not null)
        {
            GlobalPosition = _target.GlobalPosition;
        }
    }
}