using System;

namespace NineGames.Stats.Math
{
    public static class StatMath
    {
        public static int Round(float value) => (int) System.Math.Round(value, MidpointRounding.AwayFromZero);
    }
}