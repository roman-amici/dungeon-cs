using Ecs;

namespace Game;

public class CombatSystem(
    Queue<WantsToAttackMessage> attackMessages,
    Table<Damage> damages,
    Table<Health> healths) : GameSystem
{
    public override void Execute()
    {
        while(attackMessages.TryDequeue(out var attack))
        {

            var health = healths.GetEntity(attack.Attacked);
            var damage = damages.GetEntity(attack.Attacker);

            if (health is Health h && damage is Damage d)
            {
                h.CurrentHealth -= d.DamagePerHit;
                healths.Update(attack.Attacked, h);
                return;
            }
        }
    }
}

public struct WantsToAttackMessage(EntityId attacker, EntityId attacked)
{
    public EntityId Attacker {get;} = attacker;
    public EntityId Attacked {get;} = attacked;
}