using System.Collections.Generic;
using System.Linq;
using NineGames.Stats.Math;

namespace NineGames.Stats.Extensions
{
    public static partial class StatMathExtensions
    {
        public static int GetValueSum(this IEnumerable<Stat> attributes) =>
            attributes.Sum(stat => stat.GetValue());

        public static float GetMultiplierSum(this IEnumerable<Stat> attributes) =>
            1 + attributes.Sum(stat => stat.GetMultiplier() - 1);

        public static float GetMultiplierExpression(this IEnumerable<Stat> attributes) =>
            attributes.Aggregate(1f, (current, stat) => current * stat.GetMultiplier());

        public static int GetMultipliedValueSum(this IEnumerable<Stat> attributes) =>
            attributes.Sum(stat => StatMath.Round(stat.GetValue() * stat.GetMultiplier()));

        public static int GetRawSum(this IEnumerable<Stat> attributes) =>
            attributes.Sum(stat => stat.GetRawValue());

        public static int GetFinalSum(this IEnumerable<Stat> attributes) =>
            attributes.Sum(stat => stat.GetFinalValue());
    }
}