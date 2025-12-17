using Ecs;

namespace Game;

public enum GameTurn
{
    AwaitingInput,
    PlayerTurn,
    EnemyTurn
}

public class Turn
{
    public GameTurn CurrentTurn {get; set;}
    public bool EndTurn {get; set;}
}

public class EndTurnSystem(Turn turn) : GameSystem
{
    public override void Execute()
    {
        if (!turn.EndTurn)
        {
            return;
        }

        switch (turn.CurrentTurn)
        {
            case GameTurn.AwaitingInput:
                turn.CurrentTurn = GameTurn.PlayerTurn;
                
                break;

            case GameTurn.PlayerTurn:
                turn.CurrentTurn = GameTurn.EnemyTurn;
                break;

            case GameTurn.EnemyTurn:
                turn.CurrentTurn = GameTurn.AwaitingInput;
                break;
        }
    }
}