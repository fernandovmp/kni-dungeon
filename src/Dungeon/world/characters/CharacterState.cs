using System;

namespace Dungeon.world.characters;

[Flags]
public enum CharacterState
{
    None = 0,
    Idle = 1,
    Moving = 2,
    Attacking = 4,
    Hitted = 8,
    Dead = 16
}