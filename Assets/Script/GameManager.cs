public enum GameState
{
    Menu,
    Play,
    Pause
};

public enum GameMode
{
    Normal,
    Survival
};

public class GameManager : FastSingleton<GameManager>
{
    public Status status;
    public AmountPlanet AmountPlanet;

    public GameState gameCurrentState;
    public GameMode currentGameMode;

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

    public void ChangeGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;
    }
    public bool IsGameMode(GameMode gameMode)
    {
        if (currentGameMode == gameMode)
            return true;
        else
            return false;
    }

}
