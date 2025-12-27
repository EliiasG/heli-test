using Godot;
using System;

public partial class Gun : Marker3D
{
    [Export] public RigidBody3D ParentBody;
    [Export] public Node3DPool BulletPool;
    [Export] public Node3DPool ParticlePool;
    [Export] public float BulletSpeed = 100f;
    [Export] public float FireRate = 0.1f;

    private ulong _shootBegin;
    private ulong _shotCount;
    private Bullet _prev;
    

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("shoot"))
        {
            _shootBegin = Time.GetTicksMsec();
            _shotCount = 0;
            _prev = null;
        }

        if (!Input.IsActionPressed("shoot")) return;
        
        var elapsed = (Time.GetTicksMsec() - _shootBegin) / 1000f;
        var targetShotCount = (ulong)(elapsed / FireRate);
        if (targetShotCount <= _shotCount) return;
        
        var bullet = BulletPool.GetNext() as Bullet;
        bullet.GlobalTransform = GlobalTransform;
        bullet.Velocity = GlobalTransform.Basis.X * BulletSpeed + ParentBody.LinearVelocity;
        bullet.ParticlePool = ParticlePool;
        bullet.Previous = _prev;
        _prev = bullet;
        _shotCount++;
    }
}
