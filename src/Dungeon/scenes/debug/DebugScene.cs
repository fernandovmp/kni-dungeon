using System;
using System.Collections.Generic;
using System.Linq;
using FernandoVmp.GodotUtils.Scene;
using Godot;

namespace Dungeon.scenes.debug;

public partial class DebugScene : Node
{
    private Label _sceneManagerLabel;
    private List<SceneDto> _debugScenes = new List<SceneDto>();
    private int _currentSceneIndex;

    public override void _Ready()
    {
        base._Ready();
        _sceneManagerLabel = GetNode<Label>("CanvasLayer/SceneManagerPanel/Label");

        string path = "res://scenes/debug";
        DirAccess debugDirectory = DirAccess.Open(path);
        debugDirectory.ListDirBegin();
        string? entry = debugDirectory.GetNext();
        while (!string.IsNullOrEmpty(entry))
        {
            if (debugDirectory.CurrentIsDir())
            {
                entry = debugDirectory.GetNext();
                continue;
            }

            if (entry.GetExtension() == "tscn")
            {
                // string finalPath = path + "/" + entry;
                // var scene = ResourceLoader.Load<PackedScene>(finalPath);
                // var sceneDto = new SceneDto(scene, finalPath, entry.GetBaseName());
                // _debugScenes.Add(sceneDto);
            }
            
            entry = debugDirectory.GetNext();
        }

        UpdateSceneLabel();
    }

    private void UpdateSceneLabel()
    {
        _currentSceneIndex = _debugScenes.FindIndex(x => x.Path == SceneFilePath);
        if (_currentSceneIndex < 0)
        {
            return;
        }

        string currentSceneText = $"[2] Reload {_debugScenes[_currentSceneIndex].Name}";
        string previousSceneText = String.Empty;
        string nextSceneText = String.Empty;
        if (_debugScenes.Count > 1)
        {
            int previousIndex = NormalizeIndex(_currentSceneIndex - 1);
            int nextIndex = NormalizeIndex(_currentSceneIndex + 1);
            previousSceneText = $"[1] Load {_debugScenes[previousIndex].Name}";
            nextSceneText = $"[3] Load {_debugScenes[nextIndex].Name}";
        }

        _sceneManagerLabel.Text = $"{previousSceneText} {currentSceneText} {nextSceneText}";
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("debug_previous_scene"))
        {
            ChangeScene(_currentSceneIndex - 1);
        }
        if (Input.IsActionJustPressed("debug_reload_scene"))
        {
            ChangeScene(_currentSceneIndex);
        }
        if (Input.IsActionJustPressed("debug_next_scene"))
        {
            ChangeScene(_currentSceneIndex + 1);
        }
    }

    private void ChangeScene(int index)
    {
        index = NormalizeIndex(index);
        if (index >= _debugScenes.Count || index < 0)
        {
            return;
        }

        LoadScene(_debugScenes[index].Scene);
    }

    private int NormalizeIndex(int index)
    {
        if (index < 0)
        {
            index = _debugScenes.Count + index;
        }

        return index % _debugScenes.Count;
    }

    private void LoadScene(PackedScene debugScene)
    {
        SceneLoader.LoadInto(GetTree().Root, debugScene);
    }

    private record SceneDto(PackedScene Scene, string Path, string Name);
}
