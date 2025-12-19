using Ecs;

namespace Game;

public class OverlaysSystem(TurnStateScheduler gameplay, MenuScheduler menu, GameStateResource state) : GameSystem
{
    public override void Execute()
    {
        switch (state.CurrentState)
        {
            case GameState.GameRun:
                gameplay.Execute();
                break;
            default:
                menu.Execute();
                break;
        }
    }
}

public class GameStateResource(GameState state)
{
    public GameState CurrentState {get; set;} = state;
}

public enum GameState
{
    GameRun,
    PlayerWin,
    PlayerLoose
}