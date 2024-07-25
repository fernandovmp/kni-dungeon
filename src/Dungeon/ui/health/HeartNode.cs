using System;
using Godot;

namespace Dungeon.ui.health;

public partial class HeartNode : TextureRect
{
    public int Value { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        ExpandMode = ExpandModeEnum.FitWidth;
        StretchMode = StretchModeEnum.KeepAspect;
        UpdateTexture();
    }

    public void SetValue(int value)
    {
        Value = Math.Clamp(value, 0, 2);
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        string texture = null;
        if (Value == 0)
        {
            texture = "ui_heart_empty.png";
        }
        else if (Value == 1)
        {
            texture = "ui_heart_half.png";
        }
        else
        {
            texture = "ui_heart_full.png";
        }
        
        Texture = ResourceLoader.Load<Texture2D>("res://ui/health/" + texture);
    }
}