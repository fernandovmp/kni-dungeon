using System;
using Godot;

namespace Dungeon.world.characters;

public partial class CharacterNode : Node2D
{
    private Movement _movement = default!;
    private Random _random = new Random();
    private double _commandTime = 3;
    private int _command = -1;
    
    [Export] private double _speed = 4;
    
    public override void _Ready()
    {
        _movement = new Movement(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        // _commandTime -= delta;
        // if (_commandTime < 0)
        // {
        //     _commandTime = 3;
        //     _command = -1;
        // }
        // if(_command == -1)
        //     _command = _random.Next(0, 5);
        // double finalSpeed = _speed;
        // switch (_command)
        // {
        //     case 0:
        //         _movement.Move(Vector2.Down, finalSpeed);
        //         break;
        //     case 1:
        //         _movement.Move(Vector2.Up, finalSpeed);
        //         break;
        //     case 2:
        //         _movement.Move(Vector2.Right, finalSpeed);
        //         break;
        //     case 3:
        //         _movement.Move(Vector2.Left, finalSpeed);
        //         break;
        //     default:
        //         _movement.Stop();
        //         break;
        // }
    }

    public void Move(Vector2 direction)
    {
        _movement.Move(direction, _speed);
    }
}