using Dungeon.world.arena;
using FernandoVmp.GodotUtils.Services;
using Godot;

namespace Dungeon.scenes.main;

public partial class ProgressMonitorNode : Node2D
{
    private const string CacheKey = "ProgressMonitorData";
    public static void S_Reset()
    {
        var cacheService = new MemoryCacheService();
        var data = cacheService.GetValueOrDefault<ProgressData>(CacheKey);
        if (data == null)
        {
            data = new ProgressData();
            cacheService.AddOrReplace(CacheKey, data);
        }
        data.Reset();
    }

    public ProgressData GetProgress()
    {
        var cacheService = new MemoryCacheService();
        var data = cacheService.GetValueOrDefault<ProgressData>(CacheKey);
        return data ?? new ProgressData();
    }

    public void OnArenaStateChanged(ArenaState state)
    {
        if (state.State == ArenaStateEnum.WaveChange)
        {
            var data = GetProgress();
            data.TotalWaves++;
        }

        if (state.State == ArenaStateEnum.Cleared)
        {
            var data = GetProgress();
            data.TotalArenas++;
        }
    }

    public void OnArenaEnemyDied()
    {
        GD.Print("OIE");
        var data = GetProgress();
        data.TotalEnemies++;
    }
}