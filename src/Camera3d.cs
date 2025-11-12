using Godot;
using System;

public partial class Camera3d : Node3D
{
    [Export] private RigidBody3D Parent;
    [Export] private Vector3 TiltAmount;
    [Export] private Vector3 MoveAmount;
    public override void _Process(double delta)
    {
        var r = Parent.GlobalTransform.Basis.Inverse() * Parent.AngularVelocity * float.Sqrt(Parent.LinearVelocity.Length()) * 0.25f;
        var i = r * MoveAmount;
        Position = new Vector3(i.X + i.Y, 0, i.Z);
        
        r = new Vector3(-r.Z, r.Y, r.X);
        
        Rotation = r * TiltAmount;
    }
}
