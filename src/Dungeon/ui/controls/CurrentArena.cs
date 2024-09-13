using Dungeon.world.arena;
using Godot;

namespace Dungeon.ui.controls;

public partial class CurrentArena : Panel
{
    private Label _label;

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
    }

    public void SetArena(ArenaData arenaData)
    {
        _label.Text = $"Arena: {arenaData.ArenaNumber}";
    }
}