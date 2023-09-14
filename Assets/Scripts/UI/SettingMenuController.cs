using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject settingScreen;
    [SerializeField]
    private Slider mainVolumeSlider;
    [SerializeField]
    private Slider sfxVolumeSlider;
    [SerializeField]
    private Slider musicVolumeSlider;

    private bool? cursorLocked = null;

    public bool IsActive => settingScreen.activeSelf;

    private void Awake()
    {
        settingScreen.SetActive(false);
    }

    public void Toggle()
    {
        if (settingScreen.activeSelf)
        {
            BackButton();
        }
        else
        {
            Show();
        }
    }

    public void Show()
    {
        if (CursorManager.IsInstanced)
        {
            cursorLocked = CursorManager.Instance.Locked;
            CursorManager.Instance.UnlockCursor();
        }
        settingScreen.SetActive(true);
    }

    public void BackButton()
    {
        if (CursorManager.IsInstanced && cursorLocked.HasValue && cursorLocked.Value)
        {
            CursorManager.Instance.LockCursor();
        }
        settingScreen.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        SoundSettings soundSettings = SoundSettings.Instance;
        mainVolumeSlider.value = soundSettings.MainVolume;
        sfxVolumeSlider.value = soundSettings.SfxVolume;
        musicVolumeSlider.value = soundSettings.MusicVolume;
    }
}
