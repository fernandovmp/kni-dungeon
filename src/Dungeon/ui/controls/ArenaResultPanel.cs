using Dungeon.services;
using Godot;

namespace Dungeon.ui.controls;

public partial class ArenaResultPanel : Control
{
    private Control _retryControl;
    private Label _titleLabel; 
    private Label _resultsLabel;
    private Button _retryButton;
    private AudioStreamPlayer2D _audioPlayer;
    [Export] private AudioStream _clearedSound;
    [Export] private AudioStream _failedSound;
    
    public override void _Ready()
    {
        base._Ready();
        _retryControl = GetNode<Control>("VBoxContainer/Retry");
        _titleLabel = GetNode<Label>("VBoxContainer/Panel/Label");
        _resultsLabel = GetNode<Label>("VBoxContainer/Panel/HBoxContainer/ResultValues");
        _audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
    }

    public void ShowDeath(ProgressData progress) => ShowResults(progress, "Died!", _failedSound, canRetry: true);

    public void ShowCleared(ProgressData progress) => ShowResults(progress, "Cleared!", _clearedSound, canRetry: false);

    private void ShowResults(ProgressData progress, string text, AudioStream sound, bool canRetry)
    {
        Visible = true;
        _titleLabel.Text = text;
        _retryControl.Visible = canRetry;
        
        string resultValues = $"{progress.TotalDeaths}\n" +
                              $"{progress.CurrentLife}\n" +
                              $"{progress.TotalArenas}\n" +
                              $"{progress.TotalWaves}\n" +
                              $"{progress.TotalEnemies}";
        
        _resultsLabel.Text = resultValues;
        PlaySound(sound);
    }

    private void PlaySound(AudioStream sound)
    {
        if (_audioPlayer.Playing)
        {
            _audioPlayer.Stop();
        }
        _audioPlayer.Stream = sound;
        _audioPlayer.Play();
    }
}