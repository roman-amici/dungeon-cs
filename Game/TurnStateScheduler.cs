using Ecs;

namespace Game;

public class TurnStateScheduler(Turn turn) : GameSystem
{
    public List<GameSystem> AwaitingInput {get;} = new();
    public List<GameSystem> PlayerTurn {get;} = new();
    public List<GameSystem> EnemyTurn {get;} = new();

    public override void Execute()
    {
        var schedule = turn.CurrentTurn switch
        {
            GameTurn.AwaitingInput => AwaitingInput,
            GameTurn.PlayerTurn => PlayerTurn,
            GameTurn.EnemyTurn => EnemyTurn,
            _ => throw new IndexOutOfRangeException(),
        };

        foreach(var system in schedule)
        {
            system.Execute();
        }
    }
}