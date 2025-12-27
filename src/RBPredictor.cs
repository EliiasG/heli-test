using Godot;
using System;

public partial class RBPredictor : Node3D
{
	public Vector3 Velocity;
	public Vector3 AngularVelocity;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Progress((float)delta);
	}

	public void Progress(float delta)
	{
		GlobalTranslate(Velocity * delta);
        SetGlobalRotation(GlobalRotation + AngularVelocity * delta);
	}
}
