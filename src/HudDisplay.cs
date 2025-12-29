using Godot;

public partial class HudDisplay: MeshInstance3D
{
    [Export] public SubViewport SubViewport { get; set; }
    [Export] public int MaterialIndex = 0;

    public override void _Ready()
    {
        if (SubViewport == null) return;
        // --- correct way to get a ViewportTexture ---
        var viewTex = SubViewport.GetTexture();
        // --- get current effective material, but duplicate it so we don't edit a shared resource ---
        var activeMat = GetActiveMaterial(MaterialIndex) as StandardMaterial3D;
        // apply the viewport texture and set the material for this instance only
        activeMat.AlbedoTexture = viewTex;
        SetSurfaceOverrideMaterial(MaterialIndex, activeMat);
    }
}