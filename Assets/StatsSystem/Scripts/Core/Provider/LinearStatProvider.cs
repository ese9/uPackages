using NineGames.Stats.Extensions;
using NineGames.Stats.Math;

namespace NineGames.Stats
{
    public class LinearStatProvider : IStatCalculationProvider
    {
        public int CalculateStatFinalValue(Stat stat)
        {
            var value = stat.GetBaseFinalValue();
            value += stat.RawBonuses.GetFinalSum();
            value += stat.FinalBonuses.GetFinalSum();
            return StatMath.Round(value);
        }

        public int CalculateStatRawValue(Stat stat)
        {
            var value = stat.GetBaseRawValue();
            value += stat.RawBonuses.GetRawSum();
            return StatMath.Round(value);
        }
    }
}