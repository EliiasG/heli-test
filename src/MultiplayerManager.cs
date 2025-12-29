using Godot;
using System;

public partial class MultiplayerManager : Node
{
	[Export] private Helicopter _playerHeli;
	[Export] private Helicopter _otherHeli;
	
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
		Rpc(nameof(SetOtherTransform),  _playerHeli.Transform, _playerHeli.LinearVelocity, _playerHeli.AngularVelocity, _playerHeli.Throttle, _playerHeli.ThrustInput, _playerHeli.TurnDir, _playerHeli.Reversed);
	}

	public void Hit()
	{
		Rpc(nameof(HitMe));
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false,
		TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, TransferChannel = 2)]
	private void HitMe()
	{
		_playerHeli.Health--;
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false,
		TransferMode = MultiplayerPeer.TransferModeEnum.UnreliableOrdered, TransferChannel = 1)]
	private void SetOtherTransform(Transform3D transform, Vector3 velocity, Vector3 angularVelocity, float throttle, float throttleInput, Vector3 turnInput, bool reverse)
	{
		_otherHeli.Transform = transform;
		_otherHeli.LinearVelocity = velocity;
		_otherHeli.AngularVelocity = angularVelocity;
		_otherHeli.Throttle = throttle;
		_otherHeli.TurnDir = turnInput;
		_otherHeli.Reversed = reverse;
		_otherHeli.ThrustInput = throttleInput;
	}
}
