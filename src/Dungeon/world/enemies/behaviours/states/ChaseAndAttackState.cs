using Dungeon.abstractions;
using Dungeon.world.characters.commands;
using Dungeon.world.weapons;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.enemies.behaviours.states;

[GlobalClass]
public partial class ChaseAndAttackState : ChaseState
{
    [Export]
    public double AttackCooldown { get; set; }
    [Export]
    public double AttackDistance { get; set; }

    private double _timer;
    private bool _connected;
    private bool _enabled;

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_timer > 0)
        {
            _timer -= delta;
        }
    }
    
    public override void Enter()
    {
        base.Enter();
        _enabled = true;
        ConnectSignals();
    }

    public override void Update(double delta)
    {
        base.Update(delta);
        ConnectSignals();
    }

    private void ConnectSignals()
    {
        if (!_connected && Enemy?.Character != null)
        {
            var weapon = Enemy.Character.GetMetadata<WeaponNode>(nameof(WeaponNode));
            if (weapon == null)
            {
                return;
            }
            weapon.AnimationPlayer.Connect(AnimationPlayer.SignalName.AnimationFinished,
                new Callable(this, nameof(OnAnimationFinished)));
            _connected = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _enabled = false;
    }

    private void OnAnimationFinished(string animationName)
    {
        if (animationName == "attack" && _enabled)
        {
            TransitionTo("stunned");
        }
    }

    public override void PhysicsUpdate(double delta)
    {
        base.PhysicsUpdate(delta);
        if (_timer <= 0 && Player != null && Player.Character.GlobalPosition.DistanceTo(Enemy.Character.GlobalPosition) <= AttackDistance)
        {
            PerformAttack();
            _timer = AttackCooldown;
        }
    }

    private void PerformAttack()
    {
        var weapon = Enemy.Character.GetMetadata<WeaponNode>(nameof(WeaponNode));
        if (weapon != null && weapon.HasMeta("TargetPoint"))
        {
            if (Player != null)
            {
                var targetPointPath = weapon.GetMetadata<NodePath>("TargetPoint");
                var targetPoint = weapon.GetNode<Node2D>(targetPointPath);
                targetPoint.Position = weapon.GlobalPosition.DirectionTo(Player.Character.GlobalPosition);
            }
        }
        Enemy.Character.TryExecute(new CharacterAttackCommand());
    }
}