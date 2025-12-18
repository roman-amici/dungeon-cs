namespace Game;

public struct Health(double maxHealth)
{
    public double CurrentHealth {get; set;} = maxHealth;
    public double MaxHealth {get;} = maxHealth;

    public void AddHealth(double add)
    {
        CurrentHealth += add;
        CurrentHealth = Math.Min(MaxHealth, CurrentHealth);
    }
}