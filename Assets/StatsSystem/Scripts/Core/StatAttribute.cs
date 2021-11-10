namespace NineGames.Stats
{
    public abstract class StatAttribute
    {
        private readonly int value;
        private readonly float multiplier;

        protected StatAttribute(int value, float multiplier = 1f)
        {
            this.value = value;
            this.multiplier = multiplier;
        }

        public int GetValue() => value;
        public float GetMultiplier() => multiplier;
    }
}