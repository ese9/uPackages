using NineGames.Stats.Math;
using NUnit.Framework;

namespace NineGames.Stats.Tests
{
    public class DefaultStatsTest
    {
        private static IStatCalculationProvider DefaultProvider =>
            StatProviderResolver.ResolveProvider<DefaultStatProvider>();

        [Test]
        public void RawValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5);

            const int predictedResult = 5;
            var actualResult = strength.GetRawValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void FinalValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5);

            const int predictedResult = 5;
            var actualResult = strength.GetFinalValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void AddRawBonusValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5);

            var buff1 = StatFactory.CreateStat(10);
            var buff2 = StatFactory.CreateStat(7);

            strength.AddRawBonus("buff_1", buff1);
            strength.AddRawBonus("buff_2", buff2);

            const int predictedResult = 5 + 10 + 7;
            var actualResult = strength.GetRawValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void RemoveRawBonusValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5);

            var buff1 = StatFactory.CreateStat(10);
            var buff2 = StatFactory.CreateStat(7);

            strength.AddFinalBonus("buff_1", buff1);
            strength.AddFinalBonus("buff_2", buff2);

            strength.RemoveFinalBonus("buff_1");
            strength.RemoveFinalBonus("buff_2");

            const int predictedResult = 5;
            var actualResult = strength.GetRawValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void AddFinalBonusValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5);

            var strBonus1 = StatFactory.CreateStat(10);
            var strBonus2 = StatFactory.CreateStat(7);
            strength.AddFinalBonus("str_bonus_1", strBonus1);
            strength.AddFinalBonus("str_bonus_2", strBonus2);

            const int predictedResult = 5 + 10 + 7;
            var actualResult = strength.GetFinalValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void RemoveFinalBonusValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5);

            var buff1 = StatFactory.CreateStat(10);
            var buff2 = StatFactory.CreateStat(7);

            strength.AddFinalBonus("buff_1", buff1);
            strength.AddFinalBonus("buff_2", buff2);

            strength.RemoveFinalBonus("buff_1");
            strength.RemoveFinalBonus("buff_2");

            const int predictedResult = 5;
            var actualResult = strength.GetFinalValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void RawMultiplication()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5, 0.1f);

            var predictedResult = StatMath.Round(5 * 0.1f);
            var actualResult = strength.GetRawValue();
            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void FinalMultiplication()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5, 0.1f);

            var predictedResult = StatMath.Round(5 * 0.1f);
            var actualResult = strength.GetFinalValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void RawBonusMultiplication()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5, 2f);

            var strBonus1 = StatFactory.CreateStat(10, 0.5f);
            var strBonus2 = StatFactory.CreateStat(4, 1.7f);
            strength.AddRawBonus("str_bonus_1", strBonus1);
            strength.AddRawBonus("str_bonus_2", strBonus2);

            const float baseValue = 5 * 2f;
            var predictedResult = StatMath.Round((baseValue + 10 + 4) * (1 + 0.5f - 1 + 1.7f - 1));
            var actualResult = strength.GetRawValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void FinalBonusMultiplication()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5, 2f);

            var strBonus1 = StatFactory.CreateStat(10, 0.5f);
            var strBonus2 = StatFactory.CreateStat(4, 1.7f);
            strength.AddFinalBonus("str_bonus_1", strBonus1);
            strength.AddFinalBonus("str_bonus_2", strBonus2);

            const float baseValue = 5 * 2f;
            var predictedResult = StatMath.Round((baseValue + 10 + 4) * (1 + 0.5f - 1 + 1.7f - 1));
            var actualResult = strength.GetFinalValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void DependantRawValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5);
            var health = StatFactory.CreateDependantStat(100, strength, 10f);

            const int predictedResult = 100 + 5 * 10;
            var actualResult = health.GetRawValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void DependantFinalValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5);
            var health = StatFactory.CreateDependantStat(100, strength, 10);

            const int predictedResult = 100 + 5 * 10;
            var actualResult = health.GetFinalValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void DependantRawMultiplication()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5, 2f);
            var health = StatFactory.CreateDependantStat(100, 0.5f, strength, 10);

            var predictedResult = StatMath.Round(100 * 0.5f + 5 * 2f * 10);
            var actualResult = health.GetRawValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void DependantFinalMultiplication()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5, 2f);
            var health = StatFactory.CreateDependantStat(100, 0.5f, strength, 10f);

            var predictedResult = StatMath.Round(100 * 0.5f + 5 * 2f * 10);
            var actualResult = health.GetFinalValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void DependantRawBonusValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5, 2f);
            var health = StatFactory.CreateDependantStat(100, 0.5f, strength, 10f);

            var bonus1 = StatFactory.CreateStat(0, 2f);
            health.AddRawBonus("bonus_1", bonus1);

            var predictedResult = StatMath.Round((100 * 0.5f + 5 * 2f * 10) * 2f);
            var actualResult = health.GetRawValue();

            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }

        [Test]
        public void DependantFinalBonusValue()
        {
            StatFactory.CalculationProvider = DefaultProvider;

            var strength = StatFactory.CreateStat(5, 2f);
            var health = StatFactory.CreateDependantStat(100, 0.5f, strength, 10);

            var bonus1 = StatFactory.CreateStat(0, 2f);
            health.AddFinalBonus("bonus_1", bonus1);

            var predictedResult = StatMath.Round((100 * 0.5f + 5 * 2f * 10) * 2f);
            var actualResult = health.GetFinalValue();
            Assert.That(predictedResult == actualResult, $"{predictedResult} != {actualResult}");
        }
    }
}