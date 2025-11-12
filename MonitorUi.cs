using Godot;
using System;

public partial class MonitorUi : Node
{
	[Export] public Helicopter Helicopter;
	[Export] public Camera3D Camera;
	[Export] public Label SpeedLabel;
	[Export] public Label AltitudeLabel;
	[Export] public ProgressBar ThrustBar;
	[Export] public SubViewportTest Viewport;

	public override void _EnterTree()
	{
		Viewport.Cam = Camera;
	}

	public override void _Process(double delta)
	{
		var velocity = Helicopter.LinearVelocity.Length();
		var altitude = Helicopter.GlobalPosition.Y;
		SpeedLabel.Text = ((int)(velocity * 3.6f)).ToString("D3");
		AltitudeLabel.Text = ((int)altitude).ToString("D4");
		ThrustBar.Value = Helicopter.ThrustInput;
	}
}
