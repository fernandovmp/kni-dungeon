using System;
using Godot;

namespace Dungeon.world.characters;

public partial class AnimatedCharacterNode : AnimatedSprite2D
{
    public CharacterNode CharacterOwner { get; set; }

    public void RequestIdle()
    {
        Play("idle");
    }
    
    public void RequestRun()
    {
        Play("run");
    }
    
    public async void RequestHit()
    {
        const string hitAnimation = "hit";
        if (SpriteFrames.HasAnimation(hitAnimation))
        {
            Play(hitAnimation);
            CharacterOwner.State = CharacterState.Hitted;
            await ToSignal(GetTree().CreateTimer(0.6), "timeout");
            CharacterOwner.State = CharacterState.Idle;
        }
    }
    
    public void FlipSprite(Vector2 direction)
    {
        if (direction.X > 0 && FlipH)
        {
            FlipH = false;
        }
        else if (direction.X < 0 && !FlipH)
        {
            FlipH = true;
        }
    }
}