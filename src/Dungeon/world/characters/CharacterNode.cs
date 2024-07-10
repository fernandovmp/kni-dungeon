using System;
using Godot;

namespace Dungeon.world.characters;

public partial class CharacterNode : Node2D
{
    private Movement _movement = default!;
    private WeaponNode _weapon = default!;
    private double _commandTime = 3;
    private int _command = -1;
    
    [Export] private double _speed = 4;

    public override void _Ready()
    {
        _movement = new Movement(this);
        _weapon = GetNode<WeaponNode>("Body/Weapon");
    }

    public void Move(Vector2 direction)
    {
        _movement.Move(direction, _speed);
    }

    public void Attack()
    {
        _weapon.Attack(_movement.IsLookingLeft);
    }
}