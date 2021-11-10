using NineGames.Stats.Extensions;
using NineGames.Stats.Math;

namespace NineGames.Stats
{
    public class DefaultStatProvider : IStatCalculationProvider
    {
        public int CalculateStatFinalValue(Stat stat)
        {
            var value = stat.GetBaseFinalValue();
            value = (value + stat.RawBonuses.GetValueSum()) * stat.RawBonuses.GetMultiplierSum();
            value = (value + stat.FinalBonuses.GetValueSum()) * stat.FinalBonuses.GetMultiplierSum();
            return StatMath.Round(value);
        }

        public int CalculateStatRawValue(Stat stat)
        {
            var value = stat.GetBaseRawValue();
            value = (value + stat.RawBonuses.GetValueSum()) * stat.RawBonuses.GetMultiplierSum();
            return StatMath.Round(value);
        }
    }
}