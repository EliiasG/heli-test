using Godot;
using System;

public partial class Bullet : Node3D
{
    public Node3DPool ParticlePool;
    [Export] public RayCast3D RayCast3D;

    public Vector3 Velocity;

    public Bullet Previous;

    public override void _PhysicsProcess(double delta)
    {
        Velocity += Vector3.Down * 9.8f * (float)delta; // gravity
        var start = GlobalTransform.Origin;
        var end = start + Velocity * (float)delta * 2;

        var spaceState = GetWorld3D().DirectSpaceState;
        var query = new PhysicsRayQueryParameters3D
        {
            From = start,
            To = end,
            CollideWithBodies = true,
            CollideWithAreas = false,
        };

        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            var hitPos = (Vector3)result["position"];
            Visible = false;
            ProcessMode = ProcessModeEnum.Disabled;
            var particles = ParticlePool.GetNext() as GpuParticles3D;
            particles.GlobalPosition = hitPos;
            particles.Restart();
        }
        else
        {
            if (Previous is not { Visible: true })
                LookAt(GlobalPosition + Velocity);
            else
                LookAt(Previous.GlobalPosition + Previous.Velocity * 0.5f);

            GlobalPosition += Velocity * (float)delta;
        }
    }
}