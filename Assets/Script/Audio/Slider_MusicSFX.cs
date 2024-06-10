using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Slider_MusicSFX : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    public Button musicBt, SfXBt;
    public GameObject[] ban;
    private float volume, vol;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
            LoadVolume();
        else
        {
            SetMusicVolume();
            SetSfxVolume();
        }

        musicBt.onClick.AddListener(MusicBtFeature);
        SfXBt.onClick.AddListener(SfXBtFeature);
    }

    private void Update()
    {
        if (musicSlider.value < 0.01)
            ban[0].SetActive(true);
        else
            ban[0].gameObject.SetActive(false);

        if (sfxSlider.value < 0.01)
            ban[1].SetActive(true);
        else
            ban[1].SetActive(false);
    }

    public void MusicBtFeature()
    {
        if (ban[0].activeSelf)
            musicSlider.value = 0.5f;
        else
            musicSlider.value = 0.01f;
    }

    public void SfXBtFeature()
    {
        if (ban[1].activeSelf)
            sfxSlider.value = 0.5f;
        else
            sfxSlider.value = 0.01f;
    }

    public void SetMusicVolume()
    {
        volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume()
    {
        vol = sfxSlider.value;
        myMixer.SetFloat("Sfx", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("sfxVolume", vol);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSfxVolume();
    }
}
