using Dungeon.world.arena;
using Dungeon.world.player;
using Dungeon.world.waves;
using FernandoVmp.GodotUtils.Services;
using Godot;

namespace Dungeon.services;

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
        if (state.State == ArenaStateEnum.Cleared)
        {
            var data = GetProgress();
            data.TotalArenas++;
        }
    }

    public void OnWaveChanged(WaveControllerNode.WaveChangedEvent @event)
    {
        var data = GetProgress();
        data.TotalWaves++;
    }

    public void OnArenaEnemyDied()
    {
        var data = GetProgress();
        data.TotalEnemies++;
    }
    
    public void OnPlayerDied(PlayerNode playerNode)
    {
        var data = GetProgress();
        data.TotalDeaths++;
    }
}