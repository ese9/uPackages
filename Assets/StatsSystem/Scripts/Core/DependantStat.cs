namespace NineGames.Stats
{
    public class DependantStat : Stat
    {
        private readonly Stat dependantStat;
        private readonly float modificator;

        internal DependantStat(
            int value, 
            float multiplier,
            Stat dependantStat,
            float modificator,
            IStatCalculationProvider provider) :
            base(value, multiplier, provider)
        {
            this.dependantStat = dependantStat;
            this.modificator = modificator;
        }

        public override float GetBaseFinalValue() =>
            CalculateBaseValue() + CalculateDependantStatValue(dependantStat.GetFinalValue());

        public override float GetBaseRawValue() =>
            CalculateBaseValue() + CalculateDependantStatValue(dependantStat.GetRawValue());

        private float CalculateBaseValue() => (GetValue() * GetMultiplier());
        private float CalculateDependantStatValue(int value) => (value * modificator);
    }
}