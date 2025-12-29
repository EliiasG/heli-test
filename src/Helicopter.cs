using Godot;
using System;

public partial class Helicopter : RigidBody3D
{
    [Export] public float RotationForce = 20f;
    [Export] public float YawForce = 20f;
    [Export] public Node3DPool BulletPool;
    [Export] public Node3DPool DustPool;
    [Export] public Node3DPool ExplosionPool;

    [Export] public float LiftForce = 30f;

    [Export] public float ThrottleAcceleration;

    [Export] public Curve ThrottleCurve;

    [Export] public Node3D Rotor;
    [Export] public DragPoint TailRotor;
    [Export] public DragPoint RotorFront;
    [Export] public DragPoint RotorBack;
    [Export] public DragPoint RotorLeft;
    [Export] public DragPoint RotorRight;
    [Export] public Gun Gun;

    [Export] public int MaxHealth = 5;
    public int Health;


    public float ThrustInput = -1;
    public float PitchInput;
    public float RollInput;
    public float YawInput;
    public bool DisableControls;

    public bool Reversed = false;
    public float Throttle = 0f;

    private Transform3D _begin;

    public Vector3 TurnDir
    {
        get => new(PitchInput, RollInput, YawInput);
        set
        {
            PitchInput = value.X;
            RollInput = value.Y;
            YawInput = value.Z;
        }
    }

    public override void _Ready()
    {
        if (Gun == null) return;
        Gun.BulletPool = BulletPool;
        Gun.ParticlePool = DustPool;
        Health = MaxHealth;
        _begin = GlobalTransform;
        BodyEntered += BodyEnteredHandler;
    }


    public override void _PhysicsProcess(double delta)
    {
        if (DisableControls)
        {
            ThrustInput = -1;
            PitchInput = 0;
            RollInput = 0;
            YawInput = 0;
        }

        if (Health == 0)
        {
            if (ExplosionPool != null)
            {
                Rpc(nameof(Explode), GlobalPosition);
                GlobalTransform = _begin;
                GD.Print("dead");
                Health = MaxHealth;
                LinearVelocity = Vector3.Zero;
                AngularVelocity = Vector3.Zero;
            }
        }

        var inputDirection = new Vector3(
            0, //transform(Input.GetAxis("roll_right", "roll_left")),
            0,
            YawInput //transform(Input.GetAxis("yaw_left", "yaw_right"))
            //transform(Input.GetAxis("pitch_up", "pitch_down"))
        );

        // Apply thrust based on input
        var torque = GlobalTransform.Basis * inputDirection * YawForce;
        //ApplyForce(torque, TailRotor.GlobalPosition - GlobalPosition);
        TailRotor.Zero = torque;
        // Apply lift to counteract gravity
        var throttleInput = ThrottleCurve.Sample((1 + ThrustInput) * 0.5f) * 2;
        if (Reversed) throttleInput *= -1;
        Throttle = Mathf.MoveToward(Throttle, throttleInput, (float)delta * ThrottleAcceleration * 2);
        if (Input.IsActionJustPressed("reverse")) Reversed = !Reversed;
        var lift = GlobalTransform.Basis * Vector3.Up * LiftForce * Throttle;

        var rotation = new Vector3(
            PitchInput, //transform(Input.GetAxis("pitch_down", "pitch_up")),
            0,
            RollInput //transform(Input.GetAxis("roll_right", "roll_left"))
        ) * RotationForce;
        ApplyForce(GlobalTransform.Basis * rotation /** (1 + float.Abs(_throttle) * 0.75f)*/,
            Rotor.GlobalPosition - GlobalPosition);

        //ApplyForce(lift, Rotor.GlobalPosition - GlobalPosition);
        foreach (var dragPoint in new[] { RotorFront, RotorBack, RotorLeft, RotorRight })
        {
            dragPoint.Zero = lift;
        }
        /*
        RotorFront.Zero += GlobalTransform.Basis * Vector3.Up * rotation.X;
        RotorBack.Zero += GlobalTransform.Basis * Vector3.Down * rotation.X;
        RotorRight.Zero += GlobalTransform.Basis * Vector3.Up * rotation.Y;
        RotorLeft.Zero += GlobalTransform.Basis * Vector3.Down * rotation.Z;*/
    }

    private void BodyEnteredHandler(Node node)
    {
        GD.Print("collide");
        if (LinearVelocity.LengthSquared() > 5 * 5)
        {
            Health = 0;
        }
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true,
        TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, TransferChannel = 1)]
    private void Explode(Vector3 position)
    {
        var particles = ExplosionPool.GetNext() as GpuParticles3D;
        particles.GlobalPosition = position;
        particles.Restart();
    }

/*
    private float Transform(float value)
    {
        return RateCurve.Sample(float.Abs(value)) * float.Sign(value);
    }*/
}