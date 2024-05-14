using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Guide, MassText;
    [SerializeField] private Player player;

    [Header("Bt0: Continue / Bt1: Tutor / Bt2: Exit")]
    [SerializeField] private Button[] bts;
    [SerializeField] private GameObject PauseUI, TutorUI;

    private const string Guide1 = "Press <ESC> to see more";

    private void Start()
    {
        bts[0].onClick.AddListener(PauseGame);
        bts[1].onClick.AddListener(TutorFeature);
        bts[2].onClick.AddListener(() => Application.Quit());
        bts[3].onClick.AddListener(() => TutorUI.SetActive(false));
    }

    private void Update()
    {
        MassText.text = player.rb.mass.ToString();
        Guide.text = Guide1;

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    private void PauseGame()
    {
        PauseUI.SetActive(!PauseUI.activeSelf);
    }

    private void TutorFeature()
    {
        TutorUI.SetActive(true);
        PauseUI.SetActive(false);
    }
}
