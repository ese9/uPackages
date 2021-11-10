namespace NineGames.Stats
{
    public class StatFactory
    {
        public static IStatCalculationProvider CalculationProvider =
            StatProviderResolver.ResolveProvider<DefaultStatProvider>();
        
        public static readonly Stat EmptyStat = CreateStat(0);
        
        public static Stat CreateEmptyStat() => CreateStat(0);

        public static DependantStat CreateDependantStat(int value, float multiplier, Stat dependantStat, float modificator,
            IStatCalculationProvider statCalculationProvider) =>
            new DependantStat(value, multiplier, dependantStat, modificator, statCalculationProvider);

        public static DependantStat CreateDependantStat(int value, float multiplier, Stat dependantStat, float modificator) =>
            CreateDependantStat(value, multiplier, dependantStat, modificator, CalculationProvider);
        
        public static DependantStat CreateDependantStat(int value, Stat dependantStat, float modificator) =>
            CreateDependantStat(value, 1f, dependantStat, modificator, CalculationProvider);

        public static Stat CreateStat(int value, float multiplier, IStatCalculationProvider provider) =>
            new Stat(value, multiplier, provider);

        public static Stat CreateStat(int value, IStatCalculationProvider provider) =>
            CreateStat(value, 1f, provider);

        public static Stat CreateStat(int value, float multiplier = 1f) =>
            CreateStat(value, multiplier, CalculationProvider);
    }
}