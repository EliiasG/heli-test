using Godot;
using System;

public partial class DragPoint : Marker3D
{
    [Export] public float DragCoefficient = 0.1f;
    [Export] public RigidBody3D Parent;
    [Export] public Curve StallCurve;

    public override void _PhysicsProcess(double delta)
    {
        var velocity = GetVelocityAtPoint(Parent, GlobalTransform.Origin);
        var along = velocity.Dot(GlobalTransform.Basis.X);
        var ratio = velocity.LengthSquared() > 0 ? Math.Abs(along / velocity.Length()) : 0;
        along = float.Sign(along) * along * along;
        Parent.ApplyForce(GlobalTransform.Basis.X * -along * DragCoefficient / 16 * StallCurve.Sample(ratio),
            GlobalTransform.Origin - Parent.GlobalTransform.Origin);
    }

    public static Vector3 GetVelocityAtPoint(RigidBody3D rb, Vector3 point)
    {
        var r = point - rb.GlobalTransform.Origin;
        return rb.LinearVelocity + rb.AngularVelocity.Cross(r);
    }
}