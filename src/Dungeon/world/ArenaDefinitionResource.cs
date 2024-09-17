using Dungeon.world.waves;
using Godot;
using Godot.Collections;

namespace Dungeon.world;

[GlobalClass]
public partial class ArenaDefinitionResource : Resource
{
    [Export] public Array<PackedScene> Levels { get; set; }
    [Export] public Array<WaveResource> Waves { get; set; } 
    [Export] public Array<WaveResource> FinalWaves { get; set; } 
}