using System;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters.components;

public partial class CombatentNode : Node
{
    private int _maxLife;
    public int Life { get; private set; }
    public int Force { get; private set; }
    public int Resistance { get; private set; }

    [Signal]
    public delegate void DiedEventHandler();
    [Signal]
    public delegate void HittedEventHandler();
    [Signal]
    public delegate void HealedEventHandler();
    
    public bool IsAlive => Life > 0;

    public override void _EnterTree()
    {
        base._EnterTree();
        Owner.SetMetadata(nameof(CombatentNode), this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Owner.RemoveMeta(nameof(CombatentNode));
    }

    public void DealDamage()
    {
        Life -= 1;
        if (Life <= 0)
        {
            Life = 0;
            EmitSignal(SignalName.Died);
        }
        else
        {
            EmitSignal(SignalName.Hitted);
        }
    }
    
    public void Heal(int amount)
    {
        Life += amount;
        Life = Math.Min(_maxLife, Life);
        EmitSignal(SignalName.Healed);
    }

    public void Load(CharacterResource characterResource)
    {
        _maxLife = characterResource.Life;
        Life = characterResource.Life;
        Force = characterResource.Force;
        Resistance = characterResource.Resistance;
    }
}