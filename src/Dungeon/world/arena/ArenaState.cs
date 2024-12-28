using Dungeon.world.waves;
using Godot;

namespace Dungeon.world.arena;

public partial class ArenaState(ArenaStateEnum state, int waveNumber) : GodotObject
{
    public ArenaStateEnum State { get; } = state;
    public int WaveNumber { get; } = waveNumber;
}

public enum ArenaStateEnum
{
    None,
    Setup,
    WaveChange,
    Cleared
}
