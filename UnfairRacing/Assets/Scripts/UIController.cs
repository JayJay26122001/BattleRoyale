using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    public Button _mute;
    public Image _muteOn, _muteOff;
    public List<TextMeshProUGUI> playerColocations;
    public TextMeshProUGUI errorText;

    private void Start()
    {
        if (_musicSlider != null)
        {
            _musicSlider.value = AudioManager.instance.musicSource.volume;
        }
        if(_sfxSlider != null)
        {
            _sfxSlider.value = AudioManager.instance.sfxSource.volume;
        }
        if(_muteOn != null)
        {
            _muteOn.gameObject.SetActive(false);
        }
        if(_muteOff != null)
        {
            _muteOff.gameObject.SetActive(true);
        }
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
        Time.timeScale = 0;
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        Time.timeScale = 1;
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

    public void CantBuy(string message)
    {
        errorText.text = message;
        Invoke("ClearError", 3f);
    }

    public void ClearError()
    {
        errorText.text = "";
    }

    public void ColocationsHud(List<Racer> Colocations)
    {
        for(int i = 0; i < Colocations.Count; i++)
        {
            playerColocations[i].text = (i + 1) + " : " + Colocations[i].name;
        }
    }

    public void ChangeButtonColor(GameObject button)
    {
        button.GetComponent<Image>().color = new Color32(0, 125, 0, 255);
    }
}
