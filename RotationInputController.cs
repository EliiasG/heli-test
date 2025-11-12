using Godot;
using System;

public partial class RotationInputController : Node
{
    [Export] public Helicopter Helicopter;
    [Export] public Curve RateCurve;

    public override void _Process(double delta)
    {
        Helicopter.PitchInput = Transform(Input.GetAxis("pitch_down", "pitch_up"));
        Helicopter.RollInput = Transform(Input.GetAxis("roll_right", "roll_left"));
        Helicopter.YawInput = Transform(Input.GetAxis("yaw_left", "yaw_right"));
    }
    
    private float Transform(float value)
    {
        return RateCurve.Sample(float.Abs(value)) * float.Sign(value);
    }
}
