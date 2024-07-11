using Godot;

namespace Dungeon.world.characters;

public partial class CharacterResource : Resource
{
    [Export]
    public SpriteFrames Sprite { get; set; }
}