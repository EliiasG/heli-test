using Godot;
using System;

public partial class Hud : Control
{
    [Export] public RigidBody3D RB;
    [Export] private float _scaleFactor;
    
    [ExportCategory("Center")] [Export] private Color _color;
    [Export] private Font _font;

    [Export(PropertyHint.Range, "0,360,0.1,radians_as_degrees")]
    private float _angle;

    [Export] private Curve _animCurve;
    [Export] private float _animTime;

    private float _animInput;

    public override void _Process(double delta)
    {
        DisplayServer.WindowSetTitle("Helicopter test - " + Engine.GetFramesPerSecond().ToString("F0") + " FPS");
        QueueRedraw();
    }

    public override void _Draw()
    {
        
        var center = Size / 2;
        var scale = Size.Y / 648f * _scaleFactor;
        var fontSize = (int)(24f * scale);
        
        // Angle lines
        var roll = RB.Rotation.X;
        var pitch = RB.Rotation.Z;
        if (pitch > float.Pi / 2f)
            pitch -= float.Pi;
        if (pitch < -float.Pi / 2f)
            pitch += float.Pi;
        var rollDir = Vector2.Right.Rotated(roll) * scale;
        for (int i = -(90 / 5); i <= (90 / 5); i++)
        {
            var pitchOffset = i * 5f * float.Pi / 180f;
            var off = (pitch + pitchOffset) * 250f * scale;
            if (float.Abs(off) > 45f / scale)
                continue;
            //off = _angleCurve.Sample(float.Abs(off / (300 * scale))) * 300 * scale * float.Sign(off);
            var color = new Color(_color, 1f - float.Abs(off / 45f * scale));
            var upDir = Vector2.Up.Rotated(roll);
            var pitchDir = upDir * off * float.Pi * scale;
            var major = i % 2 == 0 && i != 0;
            var end = major ? 90f : 70f;
            if (i == 0)
                end = 110f;
            if (major)
            {
                DrawSetTransform(center + Vector2.Down * fontSize / 2 + pitchDir, roll);
                DrawString(_font, Vector2.Right * 80 * scale, int.Abs(i * 5).ToString(), fontSize: fontSize, modulate: color, alignment: HorizontalAlignment.Right, width: 50 * scale);
                DrawString(_font, Vector2.Left * 130 * scale, int.Abs(i * 5).ToString(), fontSize: fontSize, modulate: color, alignment: HorizontalAlignment.Left, width: 50 * scale);
                DrawSetTransform(Vector2.Zero);
            }

            if (major || i == 0)
            {
                SoftLine(center + rollDir * end + pitchDir, center + rollDir * end + pitchDir + upDir * 10f * -scale, 3f * scale, color);
                SoftLine(center - rollDir * end + pitchDir, center - rollDir * end + pitchDir + upDir * 10f * -scale, 3f * scale, color);
            }
            SoftLine(center + rollDir * 50 + pitchDir, center + rollDir * end + pitchDir, 3f * scale, color);
            SoftLine(center - rollDir * 50 + pitchDir, center - rollDir * end + pitchDir, 3f * scale, color);
            
        }

        SoftLine(center + rollDir * 40, center + rollDir * 60, 3f * scale, _color);
        SoftLine(center - rollDir * 40, center - rollDir * 60, 3f * scale, _color);
        

        // center
        var shoot = !Engine.IsEditorHint() && Input.IsActionPressed("shoot");
        var delta = (float)GetProcessDeltaTime();
        _animInput += (shoot ? delta : -delta) / _animTime;
        _animInput = float.Clamp(_animInput, 0f, 1f);
        var animValue = shoot ? _animCurve.Sample(_animInput) : 1f - _animCurve.Sample(1f - _animInput);
        var offset = float.Pi / 12f * animValue;
        SoftArc(center, 24f * scale, -_angle / 2 - offset, _angle / 2 + offset, 3f * scale);
        SoftArc(center, 24f * scale, -_angle / 2 + float.Pi - offset, _angle / 2 + float.Pi + offset, 3f * scale);
        if (animValue == 0)
            DrawCircle(center, 3f * scale, _color, antialiased: true);
        else
            DrawCircle(center, float.Lerp(2.5f, 7f, animValue) * scale, _color, false, 3f * scale, antialiased: true);
        
        //info
        /*
        DrawRect(new Rect2(center + new Vector2(-500 * scale, -250 * scale), 100 * scale, 100 * scale), _color, false, 3f * scale, true);
        DrawString(_font, center + new Vector2(-500 * scale, -205 * scale), ((int)(RB.LinearVelocity.Length() * 3.6f)).ToString("D3"), fontSize: (int)(50 * scale), modulate: _color, alignment: HorizontalAlignment.Center, width: 102 * scale);
        DrawString(_font, center + new Vector2(-500 * scale, -255 * scale), "SPD", fontSize: (int)(40 * scale), modulate: _color, alignment: HorizontalAlignment.Center, width: 102 * scale);
        
        DrawRect(new Rect2(center + new Vector2(400 * scale, -250 * scale), 100 * scale, 50 * scale), _color, false, 3f * scale, true);
        DrawString(_font, center + new Vector2(400 * scale, -205 * scale), ((int)RB.GlobalPosition.Y).ToString("D3"), fontSize: (int)(50 * scale), modulate: _color, alignment: HorizontalAlignment.Center, width: 102 * scale);
        
        DrawString(_font, center + new Vector2(400 * scale, -255 * scale), "ALT", fontSize: (int)(40 * scale), modulate: _color, alignment: HorizontalAlignment.Center, width: 102 * scale);
        */ 
    }
    

    private void SoftArc(Vector2 center, float radius, float startAngle, float endAngle, float width)
    {
        DrawArc(center, radius, startAngle, endAngle, 24, _color, width, true);
        DrawCircle(center + (Vector2.Right * radius).Rotated(endAngle + 1.2f / radius), width / 2, _color,
            antialiased: true);
        DrawCircle(center + (Vector2.Right * radius).Rotated(startAngle - 1.2f / radius), width / 2, _color,
            antialiased: true);
    }

    private void SoftLine(Vector2 from, Vector2 to, float width, Color color)
    {
        DrawLine(from, to, color, width, true);
        //DrawCircle(from, width / 2, color, antialiased: true);
        //DrawCircle(to, width / 2, color, antialiased: true);
    }
}