using NineGames.Stats.Math;
using NUnit.Framework;

namespace NineGames.Stats.Tests
{
    public class LinearStatsTest
    {
        private static IStatCalculationProvider LinearProvider => 
            StatProviderResolver.ResolveProvider<LinearStatProvider>();

        [Test]
        public void MultiplyStrength()
        {
            StatFactory.CalculationProvider = LinearProvider;
            
            var strength = StatFactory.CreateStat(5);

            var weapon = StatFactory.CreateStat(100, 1.1f);
            var weaponMasteryBonus = StatFactory.CreateStat(275, 2f);
            strength.AddFinalBonus("weapon", weapon);
            strength.AddFinalBonus("weapon_mastery", weaponMasteryBonus);

            var armor = StatFactory.CreateStat(90, 1.2f);
            var armorMasteryBonus = StatFactory.CreateStat(120, 1.5f);
            strength.AddFinalBonus("armor", armor);
            strength.AddFinalBonus("armor_mastery", armorMasteryBonus);

            var damagePotionBonus = StatFactory.CreateStat(80, 1.6f);
            strength.AddFinalBonus("damage_potion", damagePotionBonus);


            var predictedFinalValue = StatMath.Round(5 + (100 * 1.1f) + (275 * 2f) + (90 * 1.2f) + (120 * 1.5f) + (80 * 1.6f));
            var actualFinalResult = strength.GetFinalValue();

            Assert.That(predictedFinalValue == actualFinalResult, $"Predicted : {predictedFinalValue} \n Actual : {actualFinalResult}");
        }

        [Test]
        public void MultiplyHealth()
        {
            StatFactory.CalculationProvider = LinearProvider;
            
            var strength = StatFactory.CreateStat(5);

            var strBonus = StatFactory.CreateStat(15);
            strength.AddFinalBonus("str_bonus", strBonus);

            var health = StatFactory.CreateDependantStat(100, strength, 10f);

            var weapon = StatFactory.CreateStat(88, 1.1f);
            var weaponMasteryBonus = StatFactory.CreateStat(4, 2f);
            health.AddFinalBonus("weapon", weapon);
            health.AddFinalBonus("weapon_mastery", weaponMasteryBonus);

            var armor = StatFactory.CreateStat(67, 1.2f);
            var armorMasteryBonus = StatFactory.CreateStat(101, 1.5f);
            health.AddFinalBonus("armor", armor);
            health.AddFinalBonus("armor_mastery", armorMasteryBonus);

            var damagePotionBonus = StatFactory.CreateStat(90, 1.6f);
            health.AddFinalBonus("damage_potion", damagePotionBonus);

            var dependentBaseValue = StatMath.Round(100 * 1f + (5 + 15) * 10f);
            var predictedFinalValue =
                StatMath.Round(dependentBaseValue + (88 * 1.1f) + (4 * 2f) + (67 * 1.2f) + (101 * 1.5f) + (90 * 1.6f));
            var actualFinalResult = health.GetFinalValue();

            Assert.That(predictedFinalValue == actualFinalResult, $"Predicted : {predictedFinalValue} \n Actual : {actualFinalResult}");
        }
    }
}