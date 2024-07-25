using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Dungeon.ui.health;

public partial class HeartGaugeNode : HBoxContainer
{
    private int _health;
    private List<HeartNode> _hearts = new List<HeartNode>();

    [Export]
    public int Health
    {
        get => _health;
        set => SetHealth(value);
    }
    
    [Export]
    public int Count { get; set; }
    
    private void SetHealth(int value)
    {
        _health = value;
        UpdateHearts();
    }
    
    public override void _Ready()
    {
        base._Ready();
        PlaceHearts();
        UpdateHearts();
    }

    private void PlaceHearts()
    {
        for (int i = 0; i < Count; i++)
        {
            var heart = new HeartNode();
            _hearts.Add(heart);
            AddChild(heart);
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            int value = Health - (i * 2);
            _hearts[i].SetValue(value);
        }
    }
}