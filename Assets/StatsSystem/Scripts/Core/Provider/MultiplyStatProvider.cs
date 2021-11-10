using System.Collections.Generic;
using NineGames.Stats.Extensions;
using NineGames.Stats.Math;

namespace NineGames.Stats
{
    public class MultiplyStatProvider : IStatCalculationProvider
    {
        public int CalculateStatFinalValue(Stat stat)
        {
            var baseValue = stat.GetBaseFinalValue();
            AddMultipliers(ref baseValue, stat.RawBonuses);
            AddMultipliers(ref baseValue, stat.FinalBonuses);
            return StatMath.Round(baseValue);
        }

        public int CalculateStatRawValue(Stat stat)
        {
            var baseValue = stat.GetBaseRawValue();
            AddMultipliers(ref baseValue, stat.RawBonuses);
            return StatMath.Round(baseValue);
        }

        private static void AddMultipliers(ref float value, IReadOnlyCollection<Stat> statList)
        {
            var sum = statList.GetFinalSum();
            var mult = statList.GetMultiplierExpression();
            value = (value + sum) * mult;
        }
    }
}