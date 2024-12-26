using System.Collections.Generic;
using Dungeon.world.characters.components;
using Dungeon.world.player;
using FernandoVmp.GodotUtils.Extensions;
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

    public void OnPlayerReadied(PlayerNode player)
    {
        Count = player.CharacterResource.Life / 2;
        Health = player.CharacterResource.Life;
            
        PlaceHearts();
        UpdateHearts();
    }

    public void OnCombatentUpdated(PlayerNode player)
    {
        var combatent = player.Character.GetMetadata<CombatentNode>(nameof(CombatentNode));
        if (combatent != null)
        {
            Health = combatent.Life;
        }
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