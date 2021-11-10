namespace NineGames.Stats
{
    public interface IStatCalculationProvider
    {
        int CalculateStatFinalValue(Stat stat);
        int CalculateStatRawValue(Stat stat);
    }
}