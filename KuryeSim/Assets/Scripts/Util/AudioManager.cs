using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Ses Kaynakları")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Sliderlar")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // Başlangıçta sesleri slider'a göre ayarla
        if(musicSource != null)
            musicSource.volume = musicSlider.value;
        if(sfxSource != null)
            sfxSource.volume = sfxSlider.value;

        // Slider değiştikçe sesleri güncelle
        if(musicSlider != null)
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        if(sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    void UpdateMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    void UpdateSFXVolume(float value)
    {
        sfxSource.volume = value;
    }
}