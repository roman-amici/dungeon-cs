namespace Game;

public struct Health(double maxHealth)
{
    public double CurrentHealth {get; } = maxHealth;
    public double MaxHealth {get;} = maxHealth;
}