using Dungeon.services;
using Godot;

namespace Dungeon.ui.controls;

public partial class ArenaResultPanel : Control
{
    private Control _retryControl;
    private Label _titleLabel; 
    private Label _resultsLabel;
    
    public override void _Ready()
    {
        base._Ready();
        _retryControl = GetNode<Control>("VBoxContainer/Retry");
        _titleLabel = GetNode<Label>("VBoxContainer/Panel/Label");
        _resultsLabel = GetNode<Label>("VBoxContainer/Panel/HBoxContainer/ResultValues");
    }

    public void ShowDeath(ProgressData progress) => ShowResults(progress, "Died!", canRetry: true);

    public void ShowCleared(ProgressData progress) => ShowResults(progress, "Cleared!", canRetry: false);

    private void ShowResults(ProgressData progress, string text, bool canRetry)
    {
        Visible = true;
        _titleLabel.Text = text;
        _retryControl.Visible = canRetry;
        
        string resultValues = $"{progress.CurrentLife}\n" +
                              $"{progress.TotalDeaths}\n" +
                              $"{progress.TotalArenas}\n" +
                              $"{progress.TotalWaves}\n" +
                              $"{progress.TotalEnemies}";
        
        _resultsLabel.Text = resultValues;
    }
}