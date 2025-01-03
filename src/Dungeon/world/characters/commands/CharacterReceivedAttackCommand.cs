using Dungeon.abstractions;
using Dungeon.world.characters.components;
using Dungeon.world.weapons;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.characters.commands;

public class CharacterReceivedAttackCommand(WeaponBodyNode weaponBody, CharacterBodyNode body) : ICommand<CharacterBodyNode>
{
    public bool CanExecute(CharacterBodyNode target) => target.State != CharacterState.Dead;

    public void Execute(CharacterBodyNode target)
    {        
        var combatent = target.GetMetadata<CombatentNode>(nameof(CombatentNode));
        if (combatent == null)
        {
            return;
        }
        
        bool isCritical = IsCritical();
        combatent.DealDamage();
        
        ApplyKnockBack(target, isCritical, combatent);
        PlayHitSound(target, isCritical);
    }

    private void ApplyKnockBack(CharacterBodyNode target, bool isCritical, CombatentNode combatent)
    {
        var knockback = target.GetMetadata<KnockbackNode>(nameof(KnockbackNode));
        int force = GetForce(isCritical) + combatent.Force - combatent.Resistance;
        Vector2 direction = GetDirection(target);
        knockback?.Apply(new KnockbackData(direction, force));
    }

    private void PlayHitSound(CharacterBodyNode target, bool isCritical)
    {
        var weapon = body.GetMetadata<WeaponNode>(nameof(WeaponNode));
        AudioStream sound = weapon.HitSound;
        if (isCritical)
        {
            sound = weapon.CriticalSound;
        }

        if (target.HitSoundPlayer.Playing)
        {
            target.HitSoundPlayer.Stop();
        }
        target.HitSoundPlayer.Stream = sound;
        target.HitSoundPlayer.Play();
    }

    private Vector2 GetDirection(CharacterBodyNode target) => body.GlobalPosition.DirectionTo(target.GlobalPosition);

    public bool IsCritical()
    {
        var rotationDegrees = Mathf.RadToDeg(weaponBody.Sprite.Rotation);
        return rotationDegrees >= 45 && rotationDegrees <= 120;
    }
    
    private int GetForce(bool isCritical)
    {
        if (isCritical)
        {
            return 230;
        }
        return 70;
    }
}