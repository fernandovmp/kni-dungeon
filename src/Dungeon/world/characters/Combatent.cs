using System;

namespace Dungeon.world.characters;

public class Combatent
{
    public int Life { get; private set; }
    public int Force { get; private set; }
    public int Resistance { get; private set; }
    
    public Action Died { get; set; }
    public Action Hitted { get; set; }

    public void DealDamage()
    {
        Life -= 1;
        if (Life <= 0)
        {
            Life = 0;
            Died?.Invoke();
        }
        else
        {
            Hitted?.Invoke();
        }
    }

    public static Combatent From(CharacterResource characterResource) => new Combatent
    {
        Life = characterResource.Life,
        Force = characterResource.Force,
        Resistance = characterResource.Resistance
    };
}