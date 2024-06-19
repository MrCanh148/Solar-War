using UnityEngine;
using UnityEngine.UI;

public class ShowUI : FastSingleton<ShowUI>
{
    [SerializeField] private Player player1;

    [Header("Bt0: Continue / Bt1: Opotion / Bt2: Tutor / Bt3: Exit")]
    [SerializeField] private Button[] bts;
    [SerializeField] private GameObject PauseUI, GamePlayUI;
    [SerializeField] private GameObject[] UIBts; // 0: Title - 1: Option - 2: Tutor - 3: Achieve

    private bool isPaused = false;

    private void Start()
    {
        bts[0].onClick.AddListener(PauseGame);
        bts[1].onClick.AddListener(OptionBtFeature);
        bts[2].onClick.AddListener(TutorBtFeature);
        bts[3].onClick.AddListener(() => Application.Quit());
        bts[4].onClick.AddListener(BackBtFeature);
        bts[5].onClick.AddListener(AchieveBtFeature);
        bts[6].onClick.AddListener(ResetBtFeature);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && player1.canWASD)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        PauseUI.SetActive(isPaused);
        GamePlayUI.SetActive(!isPaused);
        OffAllBts();
        UIBts[0].SetActive(true);
        Time.timeScale = isPaused ? 0 : 1;
        if (isPaused)
            GameManager.instance.ChangeGameState(GameState.Pause);
        else
            GameManager.instance.ChangeGameState(GameState.InGame);
    }

    public void OptionBtFeature()
    {
        OffAllBts();
        UIBts[1].SetActive(true);
        Time.timeScale = 0;
    }

    public void TutorBtFeature()
    {
        OffAllBts();
        UIBts[2].SetActive(true);
        Time.timeScale = 0;
    }

    public void AchieveBtFeature()
    {
        OffAllBts();
        UIBts[3].SetActive(true);
        Time.timeScale = 0;
    }

    public void BackBtFeature()
    {
        Time.timeScale = 1f;
        OffAllBts();
        PauseUI.SetActive(false);
    }

    public void ResetBtFeature()
    {
        SaveManager.Instance.DeleteSaveFile();
    }

    public void OffAllBts()
    {
        foreach (GameObject go in UIBts)
            go.SetActive(false);
    }
}
