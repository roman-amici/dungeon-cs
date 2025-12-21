using Ecs;

namespace Game;

public class OverlaysSystem(GameStateResource state) : GameSystem
{
    public MenuScheduler? Menu {get; set;}
    public TurnStateScheduler? Gameplay {get; set;}

    public override void Execute()
    {
        switch (state.CurrentState)
        {
            case GameState.GameRun:
                Gameplay?.Execute();
                break;
            default:
                Menu?.Execute();
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