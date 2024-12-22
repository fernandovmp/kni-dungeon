using Dungeon.abstractions;
using Dungeon.world.characters.components;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters.commands;

public class CharacterReceivedAttackCommand(float rotationDegrees, CharacterNode body) : ICommand<CharacterBodyNode>
{
    public bool CanExecute(CharacterBodyNode target) => !target.IsInvencible
                                                        && target.State != CharacterState.Dead;

    public void Execute(CharacterBodyNode target)
    {        
        var combatent = target.GetMetadata<CombatentNode>(nameof(CombatentNode));
        if (combatent == null)
        {
            return;
        }
        
        target.SetInvecibilityAsync();
        int force = GetForce() + combatent.Force - combatent.Resistance;
        Vector2 direction = GetDirection(target);

        
        combatent.DealDamage();
        target.ApplyKnockBack(force, direction);
        PlayHitSound(target);
    }

    private void PlayHitSound(CharacterBodyNode target)
    {
        AudioStream sound = body.Weapon.HitSound;
        if (IsCritical())
        {
            sound = body.Weapon.CriticalSound;
        }

        if (target.HitSoundPlayer.Playing)
        {
            target.HitSoundPlayer.Stop();
        }
        target.HitSoundPlayer.Stream = sound;
        target.HitSoundPlayer.Play();
    }

    private Vector2 GetDirection(CharacterBodyNode target) => body.GlobalPosition.DirectionTo(target.GlobalPosition);

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