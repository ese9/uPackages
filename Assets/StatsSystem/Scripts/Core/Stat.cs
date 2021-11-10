using System.Collections.Generic;

namespace NineGames.Stats
{
    public class Stat : StatAttribute
    {
        private readonly Dictionary<string, Stat> rawBonuses = new Dictionary<string, Stat>();
        private readonly Dictionary<string, Stat> finalBonuses = new Dictionary<string, Stat>();
        private readonly IStatCalculationProvider provider;

        public IReadOnlyCollection<Stat> RawBonuses => rawBonuses.Values;
        public IReadOnlyCollection<Stat> FinalBonuses => finalBonuses.Values;

        internal Stat(
            int value,
            float multiplier,
            IStatCalculationProvider provider) :
            base(value, multiplier)
        {
            this.provider = provider;
        }

        public int GetFinalValue() => CalculateFinalValue();
        public int GetRawValue() => CalculateRawValue();

        public virtual float GetBaseRawValue() => CalculateBaseValue();
        public virtual float GetBaseFinalValue() => CalculateBaseValue();

        public void AddRawBonus(string key, Stat stat) => rawBonuses[key] = stat;
        public void RemoveRawBonus(string key) => rawBonuses.Remove(key);
        public void AddFinalBonus(string key, Stat stat) => finalBonuses[key] = stat;
        public void RemoveFinalBonus(string key) => finalBonuses.Remove(key);

        private float CalculateBaseValue() => GetValue() * GetMultiplier();

        private int CalculateRawValue() => provider.CalculateStatRawValue(this);
        private int CalculateFinalValue() => provider.CalculateStatFinalValue(this);
    }
}