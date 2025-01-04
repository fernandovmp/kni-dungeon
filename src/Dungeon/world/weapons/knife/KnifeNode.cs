using System.Collections.Generic;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.weapons.knife;


public partial class KnifeNode : WeaponNode
{
    private Node2D _origin;
    private const string TargetPointMeta = "TargetPoint";
    protected override string BodyPath => "Origin/Sprite/Body";

    public override void _Ready()
    {
        base._Ready();
        _origin = GetNode<Node2D>("Origin");
    }

    public override void Attack(bool isDirectionLeft)
    {
        var targetPointPath = this.GetMetadata<NodePath>(TargetPointMeta);
        var targetPoint = GetNode<Node2D>(targetPointPath);
        if (targetPoint != null)
        {
            var refPosition = targetPoint.Position;
            refPosition.X *= isDirectionLeft ? -1 : 1;
            _origin.Rotation = refPosition.Angle();
        }
        base.Attack(isDirectionLeft);
    }
}