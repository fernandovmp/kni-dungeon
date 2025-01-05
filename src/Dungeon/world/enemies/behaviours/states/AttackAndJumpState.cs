using Dungeon.abstractions;
using Dungeon.services.state_machine;
using Dungeon.world.characters.commands;
using Dungeon.world.weapons;
using FernandoVmp.GodotUtils.Extensions;
using Godot;

namespace Dungeon.world.enemies.behaviours.states;

[GlobalClass]
public partial class AttackAndJumpState : EnemyState
{
    
    [Export]
    public double AttackCooldown { get; set; }
    [Export]
    public double AttackDistance { get; set; }
    [Export]
    public double JumpForce { get; set; }
    [Export]
    public double JumpDelay { get; set; }
    [Export]
    public State StateWhenAttackFinished { get; private set; }

    private double _timer;
    private bool _connected;
    private bool _enabled;
    private bool _isJumping;
    private Vector2 _jumpDirection;
    private double _force;
    private double _jumpTimer;

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
        _isJumping = false;
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
            weapon.AnimationPlayer.Connect(AnimationMixer.SignalName.AnimationFinished,
                new Callable(this, nameof(OnAnimationFinished)));
            _connected = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _enabled = false;
        _isJumping = false;
        _force = 0;
        _jumpTimer = 0;
    }

    private void OnAnimationFinished(string animationName)
    {
        if (animationName == "attack" && _enabled)
        {
            if (StateWhenAttackFinished != null)
            {
                TransitionTo(StateWhenAttackFinished.Name);
            }
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

        if (_jumpTimer > 0)
        {
            _jumpTimer -= delta;
        }

        if (_jumpTimer <= 0 && !_isJumping && _enabled && _force > 0)
        {
            _isJumping = true;
        }
        if (_force > 0 && _isJumping)
        {
            var motion = _jumpDirection * (float)(_force * delta);
            _force = Mathf.Lerp(_force, 0, 0.1);
            Enemy.Character.MoveAndCollide(motion);
        }
    }

    private void PerformAttack()
    {
        var weapon = Enemy.Character.GetMetadata<WeaponNode>(nameof(WeaponNode));
        _jumpDirection = Vector2.Zero;
        if (weapon != null && weapon.HasMeta("TargetPoint"))
        {
            if (Player != null)
            {
                var targetPointPath = weapon.GetMetadata<NodePath>("TargetPoint");
                var targetPoint = weapon.GetNode<Node2D>(targetPointPath);
                targetPoint.Position = weapon.GlobalPosition.DirectionTo(Player.Character.GlobalPosition);
                _jumpDirection = targetPoint.Position;
            }
        }

        if (Enemy.Character.TryExecute(new CharacterAttackCommand()))
        {
            _isJumping = false;
            _jumpTimer = JumpDelay;
            _force = JumpForce;
        }
    }
}