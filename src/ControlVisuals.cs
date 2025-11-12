using Godot;
using System;

public partial class ControlVisuals : Node3D
{
    [Export] public Node3D Pedals;
    [Export] public Node3D Stick;
    [Export] public Node3D Collective;
    [Export] public float PedalAngle = 15f;
    [Export] public float StickAngle = 15f;
    [Export] public float CollectiveAngle = 10f;

    public override void _Process(double delta)
    {
        Pedals.RotationDegrees = new Vector3(0, Input.GetAxis("yaw_left", "yaw_right") * -PedalAngle, 0);
        Stick.RotationDegrees = new Vector3(Input.GetAxis("pitch_up", "pitch_down") * StickAngle, 0,
            Input.GetAxis("roll_right", "roll_left") * -StickAngle);
        Collective.RotationDegrees = new Vector3(Input.GetAxis("throttle_down", "throttle_up") * -CollectiveAngle, 0, 0);
    }
}
