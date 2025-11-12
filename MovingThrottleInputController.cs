using Godot;
using System;

public partial class MovingThrottleInputController : Node
{
    [Export] public Helicopter Helicopter;
    [Export] public float ThrottleSpeed = 1f;
    private float _throttle = -1f;

    public override void _Process(double delta)
    {
        float moveDelta = Input.GetAxis("throttle_move_down", "throttle_move_up");
        _throttle += moveDelta * ThrottleSpeed * (float)delta;
        _throttle = Math.Clamp(_throttle, -1f, 1f);
        Helicopter.ThrustInput = _throttle;
    }
}