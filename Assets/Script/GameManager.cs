using UnityEngine;

public enum GameState
{
    Menu,
    Play
};

public class GameManager : FastSingleton<GameManager>
{
    public Status status;
    public AmountPlanet AmountPlanet;

    public GameState gameCurrentState;

    public void ChangeState(GameState gameState)
    {
        gameCurrentState = gameState;
    }
    public bool IsState(GameState gameState)
    {
        if (gameCurrentState == gameState)
            return true;
        else
            return false;
    }
}
