using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowUI : FastSingleton<ShowUI>
{

    [SerializeField] private TextMeshProUGUI MassText;
    [SerializeField] private Character player;
    [SerializeField] private Player player1;

    [Header("Bt0: Continue / Bt1: Opotion / Bt2: Tutor / Bt3: Exit")]
    [SerializeField] private Button[] bts;
    [SerializeField] private GameObject PauseUI, GamePlayUI;
    [SerializeField] private GameObject[] UIBts; // 0: Title - 1: Option - 2: Tutor - 3: Achieve

    [SerializeField] TextMeshProUGUI NameTxt;
    [SerializeField] TextMeshProUGUI EvoluTxt;
    [SerializeField] Slider EvoluSlider;

    private bool isPaused = false;
    private int currentMass;

    private void Start()
    {
        bts[0].onClick.AddListener(PauseGame);
        bts[1].onClick.AddListener(OptionBtFeature);
        bts[2].onClick.AddListener(TutorBtFeature);
        bts[3].onClick.AddListener(() => Application.Quit());
        bts[4].onClick.AddListener(BackBtFeature);
        bts[5].onClick.AddListener(AchieveBtFeature);
        bts[6].onClick.AddListener(ResetBtFeature);
        currentMass = (int)player.rb.mass;
        UpdateInfo();
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

    public void SetNameTxt(string CharacterType)
    {
        NameTxt.text = CharacterType;
    }

    public void SetMassTxt(int mass)
    {
        float duration = 0;
        if (mass - currentMass == 1)
            duration = 0;
        else
            duration = 1;


        DOTween.To(() => currentMass, x => currentMass = x, mass, duration)
            .OnUpdate(() =>
            {
                MassText.text = currentMass.ToString();
            });
    }

    public void SetEvoluTxt(string CharacterType)
    {
        EvoluTxt.text = "To " + CharacterType;
    }

    public void SetEvoluSlider(long currentMass, long massNeedeVolution)
    {
        EvoluSlider.value = (float)currentMass / massNeedeVolution;
    }

    public void UpdateInfo()
    {
        SetNameTxt(SpawnPlanets.instance.GetNamePlanet(player.characterType));
        SetMassTxt((int)player.rb.mass);
        SetEvoluTxt((player.characterType + 1).ToString());
        SetEvoluSlider((long)player.rb.mass - SpawnPlanets.instance.GetRequiredMass(player.characterType), 
            SpawnPlanets.instance.GetRequiredMass(player.characterType + 1) - SpawnPlanets.instance.GetRequiredMass(player.characterType));
    }

    public void OffAllBts()
    {
        foreach (GameObject go in UIBts)
            go.SetActive(false);
    }
}
