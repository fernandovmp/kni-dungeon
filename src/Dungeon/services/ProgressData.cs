namespace Dungeon.services;

public class ProgressData
{
    public int CurrentLife { get; set; }
    public int TotalWaves { get; set; }
    public int TotalEnemies { get; set; }
    public int TotalArenas { get; set; }
    public int TotalDeaths { get; set; }

    public void Reset()
    {
        TotalWaves = 0;
        TotalEnemies = 0;
        TotalArenas = 0;
        TotalDeaths = 0;
        CurrentLife = 0;
    }
}