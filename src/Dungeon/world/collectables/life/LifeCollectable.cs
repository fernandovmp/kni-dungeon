using Dungeon.world.characters;
using Dungeon.world.characters.components;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.collectables.life;

public partial class LifeCollectable : Collectable
{
    private bool _collected;
    [Export] private AudioStreamPlayer2D _collectSound;

    public override void _Ready()
    {
        base._Ready();
        _collectSound.Connect(AudioStreamPlayer2D.SignalName.Finished, new Callable(this, nameof(OnCollectFinished)));
    }

    public override void OnCollected(Node2D collector)
    {
        if (collector is CharacterBodyNode character && !character.IsEnemy && !_collected)
        {
            _collected = true;
            Visible = false;
            var combatentNode = character.GetMetadata<CombatentNode>(nameof(CombatentNode));
            combatentNode?.Heal(2);
            _collectSound.Play();
        }
    }

    public void OnCollectFinished()
    {
        QueueFree();
    }
}