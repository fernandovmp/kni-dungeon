using Dungeon.abstractions;
using Dungeon.world.characters.components;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters.commands;

public class CharacterReceivedAttackCommand(float rotationDegrees, CharacterNode body) : ICommand<CharacterNode>
{
    public bool CanExecute(CharacterNode target) => !target.Body.IsInvencible
                                                    && target.State != CharacterState.Dead;

    public void Execute(CharacterNode target)
    {        
        var combatent = target.Body.GetMetadata<CombatentNode>(nameof(CombatentNode));
        if (combatent == null)
        {
            return;
        }
        
        target.Body.SetInvecibilityAsync();
        int force = GetForce() + combatent.Force - combatent.Resistance;
        Vector2 direction = GetDirection(target);

        
        combatent.DealDamage();
        target.Body.ApplyKnockBack(force, direction);
        PlayHitSound(target);
    }

    private void PlayHitSound(CharacterNode target)
    {
        AudioStream sound = body.Weapon.HitSound;
        if (IsCritical())
        {
            sound = body.Weapon.CriticalSound;
        }

        if (target.Body.HitSoundPlayer.Playing)
        {
            target.Body.HitSoundPlayer.Stop();
        }
        target.Body.HitSoundPlayer.Stream = sound;
        target.Body.HitSoundPlayer.Play();
    }

    private Vector2 GetDirection(CharacterNode target) => body.Body.GlobalPosition.DirectionTo(target.Body.GlobalPosition);

    public bool IsCritical() => rotationDegrees >= 45 && rotationDegrees <= 120;
    
    private int GetForce()
    {
        if (IsCritical())
        {
            return 230;
        }
        return 70;
    }
}