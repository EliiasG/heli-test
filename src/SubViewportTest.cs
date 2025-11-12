using Godot;
using System;

public partial class SubViewportTest : SubViewport
{
    [Export] public Camera3D Cam;
    private Camera3D myCam;

    public override void _Ready()
    {
        World3D = Cam.GetWorld3D();
        myCam = Cam.Duplicate() as Camera3D;
        AddChild(myCam);
        myCam.MakeCurrent();
    }

    public override void _Process(double delta)
    {
        myCam.GlobalTransform = Cam.GlobalTransform;
    }
}
