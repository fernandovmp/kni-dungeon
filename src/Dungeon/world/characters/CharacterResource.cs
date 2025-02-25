using Godot;

namespace Dungeon.world.characters;

[GlobalClass]
[Tool]
public partial class CharacterResource : Resource
{
    [Export]
    public SpriteFrames Sprite { get; set; }

    [Export]
    public int Life { get; set; } = 2;

    [Export]
    public int Force { get; set; } = 10;

    [Export]
    public int Resistance { get; set; }
    
    [Export]
    public int Speed { get; set; }
    
    [Export]
    public PackedScene Weapon { get; set; }
    
    [Export]
    public float LifeDropChance { get; set; }
}