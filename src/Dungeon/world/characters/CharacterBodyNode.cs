using Godot;

namespace Dungeon.world.characters;

public partial class CharacterBodyNode : CharacterBody2D
{
    public Movement Movement { get; private set; }
    public bool IsInvencible { get; private set; }
    
    public override void _Ready()
    {
        Movement = new Movement(this);
    }

    public async void SetInvecibilityAsync()
    {
        if(IsInvencible) return;
        IsInvencible = true;
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        IsInvencible = false;
    }

}