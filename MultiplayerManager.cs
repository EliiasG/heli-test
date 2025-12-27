using Godot;
using System;

public partial class MultiplayerManager : Node
{
	[Export] private RigidBody3D _playerRB;
	[Export] private RBPredictor _otherPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if(Multiplayer.GetPeers().Length != 1) return;
		Rpc(nameof(SetOtherTransform),  _playerRB.Transform, _playerRB.LinearVelocity, _playerRB.AngularVelocity);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false,
		TransferMode = MultiplayerPeer.TransferModeEnum.UnreliableOrdered, TransferChannel = 1)]
	private void SetOtherTransform(Transform3D transform, Vector3 velocity, Vector3 angularVelocity)
	{
		GD.Print(transform.Origin);
		_otherPlayer.Transform = transform;
		_otherPlayer.Velocity = velocity;
		_otherPlayer.AngularVelocity = angularVelocity;
	}
}
