using UnityEngine;

public enum GameState
{
    Menu,
    InGame,
    Pause,
    GameOver
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
    public float timePlay;

    public void ChangeGameState(GameState gameState)
    {
        if (gameState == GameState.GameOver)
        {
            //Goi su kien ket thuc game
        }
        gameCurrentState = gameState;
    }
    public bool IsGameState(GameState gameState)
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

    private void Start()
    {
        timePlay = 0;
        ChangeGameState(GameState.Menu);
    }

    private void Update()
    {
        if (IsGameState(GameState.InGame))
        {
            timePlay += Time.deltaTime;
        }

        if (IsGameMode(GameMode.Survival))
        {
            if (timePlay >= 0 && timePlay < 120)   //CharacterType.Asteroid
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.Asteroid);
            }
            else if (timePlay < 240) //CharacterType.SmallPlanet
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.SmallPlanet);
            }
            else if (timePlay < 360)  //CharacterType.LifePlanet
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.LifePlanet);
            }
            else if (timePlay < 480) //CharacterType.GasGiantPlanet
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.GasGiantPlanet);
            }
            else if (timePlay < 600)  //CharacterType.SmallStar
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.SmallStar);
            }
            else if (timePlay < 720)  //CharacterType.MediumStar
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.MediumStar);
            }
            else if (timePlay < 840)  //CharacterType.bigStar
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.BigStar);
            }
            else if (timePlay < 960) //CharacterType.NeutronStar
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.NeutronStar);
            }
            else if (timePlay < 1080)  //CharacterType.BlackHole
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.BlackHole);
            }
            else if (timePlay < 1200)   //CharacterType.BigCrunch
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.BigCrunch);
            }
            else
            {
                ChangeGameState(GameState.GameOver);
            }
        }
    }

}
