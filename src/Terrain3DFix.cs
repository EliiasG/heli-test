using Godot;
using System;

public partial class Terrain3DFix : Node
{
    public override void _Ready()
    {
        base._Ready();
        SetMultimeshChildrenLayers(this, 2);
    }

    private void SetMultimeshChildrenLayers(Node root, uint layerMask)
    {
        foreach (Node child in root.GetChildren())
        {
            if (child is MultiMeshInstance3D mmi)
                mmi.Layers = layerMask;

            if (child.GetChildCount() > 0)
                SetMultimeshChildrenLayers(child, layerMask);
        }
    }
}
