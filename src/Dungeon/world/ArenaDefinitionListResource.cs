using System;
using System.Linq;
using Dungeon.world.arena;
using Godot;
using Godot.Collections;

namespace Dungeon.world;

public partial class ArenaDefinitionListResource : Resource
{
    [Export] public Array<ArenaDefinitionResource> Arenas { get; set; }
    
    public static ArenaData S_CreateArena(int index, ArenaData previousData)
    {
        var definitionList = ResourceLoader.Load<ArenaDefinitionListResource>("res://world/arenas_list.tres");
        return definitionList.CreateArena(index, previousData);
    }
    
    public ArenaData CreateArena(int index, ArenaData previousData)
    {
        var definition = Arenas[index];
        var data = new ArenaData();
        var random = new Random();
        int retry = -1;
        do
        {
            retry++;
            var level = definition.Levels[random.Next(definition.Levels.Count)];
            string[] waves = new string[3];
            for (int i = 0; i < 2; i++)
            {
                waves[i] = definition.Waves[random.Next(definition.Waves.Count)].ResourcePath;
            }
            waves[2] = definition.Waves[random.Next(definition.FinalWaves.Count)].ResourcePath;
            data.Level = level.ResourcePath;
            data.WavesResources = waves;
        } while (retry < 3 && previousData != null && (data.Level == previousData.Level || !data.WavesResources.Any(x => previousData.WavesResources.Contains(x))));

        return data;
    }
}