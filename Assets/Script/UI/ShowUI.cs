using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowUI : FastSingleton<ShowUI>
{

    [SerializeField] private TextMeshProUGUI Guide, MassText;
    [SerializeField] private Character player;

    [Header("Bt0: Continue / Bt1: Tutor / Bt2: Exit")]
    [SerializeField] private Button[] bts;
    [SerializeField] private GameObject PauseUI, TutorUI;
    [SerializeField] GameObject SettingUI;

    [SerializeField] TextMeshProUGUI NameTxt;
    [SerializeField] TextMeshProUGUI EvoluTxt;
    [SerializeField] Slider EvoluSlider;


    private const string Guide1 = "Press <ESC> to see more";
    private bool isPaused = false;

    private void Start()
    {
        bts[0].onClick.AddListener(PauseGame);
        bts[1].onClick.AddListener(TutorBtFeature);
        bts[2].onClick.AddListener(() => Application.Quit());
        bts[3].onClick.AddListener(BackBtFeature);
        UpdateInfo();
    }

    private void Update()
    {
        //MassText.text = player.rb.mass.ToString();
        Guide.text = Guide1;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        PauseUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void TutorBtFeature()
    {
        TutorUI.SetActive(true);
        PauseUI.SetActive(false);
        Time.timeScale = 0;
    }

    public void BackBtFeature()
    {
        Time.timeScale = 1f;
        TutorUI.SetActive(false);
    }

    public void SetNameTxt(string CharacterType)
    {
        NameTxt.text = CharacterType;
    }

    public void SetMassTxt(int mass)
    {
        MassText.text = mass.ToString();
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
        /*int nestMass = 0;
        foreach (var c in SpawnPlanets.instance.CharacterInfos)
        {
            if (c.characterType == player.characterType + 1)
            {
                nestMass = c.requiredMass;
            }
        }*/
        SetEvoluSlider((long)player.rb.mass - SpawnPlanets.instance.GetRequiredMass(player.characterType), SpawnPlanets.instance.GetRequiredMass(player.characterType + 1) - SpawnPlanets.instance.GetRequiredMass(player.characterType));
    }

    public void ShowSettingUI()
    {
        bool active = SettingUI.activeSelf;
        if (active)
        {
            SettingUI.SetActive(!active);
            GameManager.instance.ChangeState(GameState.Play);
        }
        else
        {
            SettingUI.SetActive(!active);
            GameManager.instance.ChangeState(GameState.Menu);
        }

    }
}
