using Godot;
using System;

public partial class DragPoint : Marker3D
{
    [Export] public float DragCoefficient = 0.1f;
    [Export] public RigidBody3D Parent;
    public Vector3 Zero;

    public override void _PhysicsProcess(double delta)
    {
        var velocity = GetVelocityAtPoint(Parent, GlobalTransform.Origin) - Zero;
        var along = velocity.Dot(GlobalTransform.Basis.X);
        along *= velocity.Length();
        Parent.ApplyForce(GlobalTransform.Basis.X * -along * DragCoefficient / 16,
            GlobalTransform.Origin - Parent.GlobalTransform.Origin);
    }

    public static Vector3 GetVelocityAtPoint(RigidBody3D rb, Vector3 point)
    {
        var r = point - rb.GlobalTransform.Origin;
        return rb.LinearVelocity + rb.AngularVelocity.Cross(r);
    }
}