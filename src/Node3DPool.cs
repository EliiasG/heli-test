using Godot;
using System;
using System.Collections.Generic;

public partial class Node3DPool : Node
{
    [Export]
    public PackedScene Scene;
    [Export]
    public int PoolSize = 10;

    private Node3D[] _nodes;

    private int _idx;

    public override void _Ready()
    {
        _nodes = new Node3D[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            var node = Scene.Instantiate<Node3D>();
            node.Visible = false;
            node.ProcessMode = ProcessModeEnum.Disabled;
            AddChild(node);
            _nodes[i] = node;
        }
    }

    public Node3D GetNext()
    {
        var node = _nodes[_idx];
        _idx = (_idx + 1) % PoolSize;
        node.Visible = true;
        node.ProcessMode = ProcessModeEnum.Inherit;
        return node;
    }
}
