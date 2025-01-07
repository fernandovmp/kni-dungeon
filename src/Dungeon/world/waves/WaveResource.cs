using Dungeon.world.characters;
using Godot;
using Godot.Collections;

namespace Dungeon.world.waves;

[GlobalClass]
public partial class WaveResource : Resource
{
    [Export] public Array<PackedScene> Enemies { get; set; }
}