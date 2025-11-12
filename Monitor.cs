using Godot;
using System;

public partial class Monitor : Node3D
{
    [Export] public Helicopter Helicpter;
    [Export] public MonitorUi MonitorUi;
    
    public override void _Ready()
    {
        MonitorUi.Helicopter = Helicpter;
    }
}
