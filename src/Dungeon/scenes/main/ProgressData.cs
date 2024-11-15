namespace Dungeon.scenes.main;

public class ProgressData
{
    public int TotalWaves { get; set; }
    public int TotalEnemies { get; set; }
    public int TotalArenas { get; set; }

    public void Reset()
    {
        TotalWaves = 0;
        TotalEnemies = 0;
        TotalArenas = 0;
    }
}