using System;
using System.Collections.Generic;

namespace NineGames.Stats
{
    public static class StatProviderResolver
    {
        private static readonly Dictionary<Type, IStatCalculationProvider> resolvedProviders;

        static StatProviderResolver()
        {
            resolvedProviders = new Dictionary<Type, IStatCalculationProvider>();
        }

        public static TProvider ResolveProvider<TProvider>() where TProvider : class, IStatCalculationProvider, new()
        {
            if (resolvedProviders.ContainsKey(typeof(TProvider)))
                return resolvedProviders[typeof(TProvider)] as TProvider;

            var provider = new TProvider();
            resolvedProviders.Add(typeof(TProvider), provider);
            return provider;
        }
    }
}