using NineGames.Stats.Math;
using NUnit.Framework;

namespace NineGames.Stats.Tests
{
    public class MultiplyStatsTest
    {
        private static IStatCalculationProvider MultiplyProvider =>
            StatProviderResolver.ResolveProvider<MultiplyStatProvider>();

        [Test]
        public void GameSimulationTest1()
        {
            StatFactory.CalculationProvider = MultiplyProvider;

            var strength = StatFactory.CreateStat(7);

            var health = StatFactory.CreateDependantStat(100, 1f, strength, 25);

            var weapon = StatFactory.CreateStat(1);
            var weaponMastery = StatFactory.CreateStat(0, 1.1f);
            var weaponTier = StatFactory.CreateStat(0, 4f);
            weapon.AddFinalBonus("weapon_mastery", weaponMastery);
            weapon.AddFinalBonus("weapon_tier", weaponTier);

            health.AddFinalBonus("weapon", weapon);

            var baseStat = StatMath.Round(100 + 7 * 25);
            var weaponBonus = StatMath.Round(1 * 1.1f * 4f);
            baseStat += weaponBonus;
            var actualFinalResult = health.GetFinalValue();

            Assert.That(baseStat == actualFinalResult, $"Predicted : {baseStat}  \n Actual : {actualFinalResult}");
        }

        [Test]
        public void MultiplyStrength()
        {
            StatFactory.CalculationProvider = MultiplyProvider;

            var strength = StatFactory.CreateStat(5);

            var weapon = StatFactory.CreateStat(1, 1.1f); // оружие даёт 10%
            var weaponMasteryBonus = StatFactory.CreateStat(0, 2f); // мастери даёт 100%
            strength.AddFinalBonus("weapon_mastery", weaponMasteryBonus);
            strength.AddFinalBonus("weapon", weapon);

            var armor = StatFactory.CreateStat(0, 1.2f); // броня даёт 20%
            var armorMasteryBonus = StatFactory.CreateStat(0, 1.5f); // мастери даёт 50%
            strength.AddFinalBonus("armor", armor);
            strength.AddFinalBonus("armor_mastery", armorMasteryBonus);

            var damagePotionBonus = StatFactory.CreateStat(0, 1.6f); // бонус зелья 60%
            strength.AddFinalBonus("damage_potion", damagePotionBonus);

            var predictedFinalValue = StatMath.Round((5 + 1) * 1.1f * 2f * 1.2f * 1.5f * 1.6f);
            var actualFinalResult = strength.GetFinalValue();

            Assert.That(predictedFinalValue == actualFinalResult, $"Predicted : {predictedFinalValue} \n Actual : {actualFinalResult}");
        }

        [Test]
        public void MultiplyHealth()
        {
            StatFactory.CalculationProvider = MultiplyProvider;

            var strength = StatFactory.CreateStat(5);

            var strBonus1 = StatFactory.CreateStat(15);
            var strBonus2 = StatFactory.CreateStat(0, 2f);
            var strBonus3 = StatFactory.CreateStat(0, 1.5f);

            strength.AddFinalBonus("bonus_1", strBonus1);
            strength.AddFinalBonus("bonus_2", strBonus2);
            strength.AddFinalBonus("bonus_3", strBonus3);

            var health = StatFactory.CreateDependantStat(100, strength, 10f);

            var weapon = StatFactory.CreateStat(0, 1.1f); // оружие даёт 10%
            var weaponMasteryBonus = StatFactory.CreateStat(0, 2f); // мастери даёт 100%
            health.AddFinalBonus("weapon", weapon);
            health.AddFinalBonus("weapon_mastery", weaponMasteryBonus);

            var armor = StatFactory.CreateStat(0, 1.2f); // броня даёт 20%
            var armorMasteryBonus = StatFactory.CreateStat(0, 1.5f); // мастери даёт 50%
            health.AddFinalBonus("armor", armor);
            health.AddFinalBonus("armor_mastery", armorMasteryBonus);

            var damagePotionBonus = StatFactory.CreateStat(0, 1.6f); // бонус зелья 60%
            health.AddFinalBonus("damage_potion", damagePotionBonus);

            var predictedFinalValue = StatMath.Round((100 + (5 + 15) * 2 * 1.5f * 10) * 1.1f * 2f * 1.2f * 1.5f * 1.6f);
            var actualFinalResult = health.GetFinalValue();

            Assert.That(predictedFinalValue == actualFinalResult, $"Predicted : {predictedFinalValue}  \n Actual : {actualFinalResult}");
        }
    }
}