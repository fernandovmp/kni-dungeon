using Godot;

namespace FernandoVmp.GodotUtils.Extensions;

public static class NodeExtensions
{
    public static SignalAwaiter WaitForSeconds(this Node node, double seconds)
        => node.ToSignal(node.GetTree().CreateTimer(seconds), "timeout");
    
    public static void SetMetadata<T>(this Node node, string key, T value) where T : Node
    {
        node.SetMeta(key, value);
    }

    public static T? GetMetadata<T>(this Node node, string key)
    {
        if (!node.HasMeta(key))
        {
            return default;
        }
        var meta = node.GetMeta(key);
        if (meta.Obj is T value)
        {
            return value;
        }
        
        return default;
    }
}