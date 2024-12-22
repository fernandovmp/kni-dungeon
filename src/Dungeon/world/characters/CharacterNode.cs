using Dungeon.abstractions;
using Dungeon.world.characters.components;
using Dungeon.world.constants;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters;

public partial class CharacterNode : Node2D
{
    [Export] public bool IsEnemy { get; set; }
    
    public CharacterBodyNode Body { get; private set;  }
    public WeaponNode Weapon { get; private set; }

    public override void _Ready()
    {
        Weapon = GetNode<WeaponNode>("Body/Weapon");
        Body = GetNode<CharacterBodyNode>("Body");
    }

    public void Configure(CharacterResource character)
    {
        Body.Configure(character, IsEnemy);
        Weapon.Configure(IsEnemy, Body);
    }
}