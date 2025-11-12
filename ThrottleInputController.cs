using Godot;
using System;

public partial class ThrottleInputController : Node
{
    [Export] public Helicopter Helicopter;
    
    public override void _Process(double delta)
    {
        Helicopter.ThrustInput = Input.GetAxis("throttle_down", "throttle_up");
    }
}
