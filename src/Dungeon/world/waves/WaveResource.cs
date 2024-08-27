using Dungeon.world.characters;
using Godot;
using Godot.Collections;

namespace Dungeon.world.waves;

public partial class WaveResource : Resource
{
    [Export] public Array<CharacterResource> Enemies { get; set; }
}