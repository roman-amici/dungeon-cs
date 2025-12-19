using System.Drawing;
using Drawing;
using Ecs;

namespace Game;

public class MenuScheduler(GameStateResource state, ViewPort viewPort, TextRenderer text, Screen screen) : GameSystem
{
    public override void Execute()
    {
        var center = viewPort.Center;
        switch (state.CurrentState)
        {
            case GameState.PlayerWin:
                screen.SetBackground(Color.Black);
                text.DrawTextCentered(new("You Win!", 24, Color.White), center);
                break;
            case GameState.PlayerLoose:
                screen.SetBackground(Color.DarkRed);
                text.DrawTextCentered(new("You Loose!", 24, Color.White), center);
                break;
        }
    }
}