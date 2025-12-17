namespace Game;

public struct Health(double maxHealth)
{
    public double CurrentHealth {get; set;} = maxHealth;
    public double MaxHealth {get;} = maxHealth;
}