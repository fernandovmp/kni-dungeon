using System;
using System.Collections.Generic;
using Godot;

namespace Dungeon.services.context_map;

public partial class ContextMapNode : Node2D
{
    private List<RayCast2D> _directions = new List<RayCast2D>(8);

    [Export] public int DangerValue { get; set; } = 5;
    [Export] public int AdjacentDangerValue { get; set; } = 2;
    public override void _Ready()
    {
        base._Ready();
        foreach (var child in GetChildren())
        {
            if (child is RayCast2D node2D)
            {
                _directions.Add(node2D);
            }
        }
    }

    public Vector2 GetDesiredDirection(Vector2 direction)
    {
        var contextMap = GetContextMap(direction);
        int maxIndex = -1;
        float max = float.MinValue;
        for (int index = 0; index < contextMap.Length; index++)
        {
            if (contextMap[index] > max)
            {
                max = contextMap[index];
                maxIndex = index;
            }
        }
        return _directions[maxIndex].Position;
    }

    public float[] GetContextMap(Vector2 desiredDirection)
    {
        desiredDirection = desiredDirection.Normalized();
        var interests = GetInterestVectors(desiredDirection);
        GD.Print($"interests: {string.Join(',', interests)}");
        var dangers = GetDangerVectors();
        GD.Print($"dangers: {string.Join(',', dangers)}");
        var contextMap = new float[interests.Length];
        for (int i = 0; i < interests.Length; i++)
        {
            contextMap[i] = interests[i] - dangers[i];
        }
        GD.Print($"contextMap: {string.Join(',', contextMap)}");
        return contextMap;
    }

    public float[] GetInterestVectors(Vector2 direction)
    {
        var interestVectors = new float[_directions.Count];
        for (var index = 0; index < _directions.Count; index++)
        {
            var directionNode = _directions[index];
            interestVectors[index] = direction.Dot(directionNode.Position);
        }
        return interestVectors;
    }

    public float[] GetDangerVectors()
    {
        var dangerVectors = new float[_directions.Count];
        for (var index = 0; index < _directions.Count; index++)
        {
            var directionNode = _directions[index];
            float value = dangerVectors[index];
            if (directionNode.IsColliding())
            {
                value = DangerValue;
                int backIndex = GetSafeIndex(index - 1, dangerVectors.Length);
                dangerVectors[backIndex] = Math.Max(dangerVectors[backIndex], AdjacentDangerValue);
                int nextIndex = GetSafeIndex(index + 1, dangerVectors.Length);
                dangerVectors[nextIndex] = Math.Max(dangerVectors[nextIndex], AdjacentDangerValue);
            }
            
            dangerVectors[index] = value;
        }

        return dangerVectors;
    }

    private int GetSafeIndex(int index, int count)
    {
        if (index < 0)
        {
            return count + index;
        }

        return index % count;
    }
}