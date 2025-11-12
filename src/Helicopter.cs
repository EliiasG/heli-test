using Godot;
using System;

public partial class Helicopter : RigidBody3D
{
    [Export] public float RotationForce = 20f;
    [Export] public float YawForce = 20f;
    [Export] public Node3DPool BulletPool;
    [Export] public Node3DPool DustPool;

    [Export] public float LiftForce = 30f;

    [Export] public float ThrottleAcceleration;
    

    [Export] public Curve RateCurve;

    [Export] public Curve ThrottleCurve;

    [Export] public Node3D Rotor;
    [Export] public Node3D TailRotor;
    
    [Export] public Gun Gun;


    private bool _reversed = false;
    private float _throttle = 0f;
    
    public override void _Ready()
    {
        Gun.ParentBody = this;
        Gun.BulletPool = BulletPool;
        Gun.ParticlePool = DustPool;
    }


    public override void _PhysicsProcess(double delta)
    {
        var inputDirection = new Vector3(
            0, //transform(Input.GetAxis("roll_right", "roll_left")),
            0,
            transform(Input.GetAxis("yaw_left", "yaw_right"))
            //transform(Input.GetAxis("pitch_up", "pitch_down"))
        );

        // Apply thrust based on input
        var torque = GlobalTransform.Basis * inputDirection * RotationForce;
        ApplyForce(torque, TailRotor.GlobalPosition - GlobalPosition);
        // Apply lift to counteract gravity
        var throttleInput = ThrottleCurve.Sample((1 + Input.GetAxis("throttle_down", "throttle_up")) * 0.5f) * 2;
        if (_reversed) throttleInput *= -1;
        _throttle = Mathf.MoveToward(_throttle, throttleInput, (float)delta * ThrottleAcceleration * 2);
        if (Input.IsActionJustPressed("reverse")) _reversed = !_reversed;
        var lift = GlobalTransform.Basis * Vector3.Up * LiftForce * _throttle;

        var inputDir = new Vector3(
            transform(Input.GetAxis("pitch_down", "pitch_up")),
            0,
            transform(Input.GetAxis("roll_right", "roll_left"))
        );
        ApplyForce(lift, Rotor.GlobalPosition - GlobalPosition);
        ApplyForce(GlobalTransform.Basis * inputDir * RotationForce * (1 + float.Abs(_throttle) * 0.75f), Rotor.GlobalPosition - GlobalPosition);
    }


    private float transform(float value)
    {
        return RateCurve.Sample(float.Abs(value)) * float.Sign(value);
    }
}