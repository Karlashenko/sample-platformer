namespace Sample.Values
{
    public readonly struct Healing
    {
        public readonly int Amount;
        public readonly HealingType Type;

        public Healing(int amount, HealingType type)
        {
            Amount = amount;
            Type = type;
        }
    }
}
