using Godot;
using System;

public partial class Gun : Marker3D
{
    [Export] public Node3DPool BulletPool;
    [Export] public Node3DPool ParticlePool;
    [Export] public float BulletSpeed = 100f;
    [Export] public float FireRate = 0.1f;

    private ulong _shootBegin;
    private ulong _shotCount;
    

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("shoot"))
        {
            _shootBegin = Time.GetTicksMsec();
            _shotCount = 0;
        }

        if (!Input.IsActionPressed("shoot")) return;
        
        var elapsed = (Time.GetTicksMsec() - _shootBegin) / 1000f;
        var targetShotCount = (ulong)(elapsed / FireRate);
        if (targetShotCount <= _shotCount) return;
        
        Rpc(nameof(SpawnBulletRpc), GlobalTransform);
        SpawnBullet(GlobalTransform, true);
        _shotCount++;
    }

    private void SpawnBullet(Transform3D transform, bool active)
    {
        var bullet = BulletPool.GetNext() as Bullet;
        bullet.GlobalTransform = transform;
        bullet.Velocity = transform.Basis.X * BulletSpeed;
        bullet.ParticlePool = ParticlePool;
        bullet.Active = active;
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false,
        TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, TransferChannel = 1)]
    private void SpawnBulletRpc(Transform3D transform)
    {
        SpawnBullet(transform, false);
    }
}
