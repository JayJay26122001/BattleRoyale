using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    public Button _mute;
    public Image _muteOn, _muteOff;

    private void Start()
    {
        _musicSlider.value = AudioManager.instance.musicSource.volume;
        _sfxSlider.value = AudioManager.instance.sfxSource.volume;
        _muteOn.gameObject.SetActive(false);
        _muteOff.gameObject.SetActive(true);
        GameManager.manager.uiController = this;
    }

    private void Update()
    {
        UpdateMuteButtonImage();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void ToggleMusicAndSoundEffects()
    {
        AudioManager.instance.ToggleMusicAndSoundEffects();
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicSlider.value);
    }

    public void SoundEffectsVolume()
    {
        AudioManager.instance.SoundEffectsVolume(_sfxSlider.value);
    }

    public void UpdateMuteButtonImage()
    {
        if (AudioManager.instance.musicSource.mute && AudioManager.instance.sfxSource.mute)
        {
            _muteOn.gameObject.SetActive(true);
            _muteOff.gameObject.SetActive(false);
        }
        else
        {
            _muteOn.gameObject.SetActive(false);
            _muteOff.gameObject.SetActive(true);
        }
    }
}
