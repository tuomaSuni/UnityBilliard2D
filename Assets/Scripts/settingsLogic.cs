using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class settingsLogic : MonoBehaviour
{
    [SerializeField] Slider songSlider;
    [SerializeField] Slider soundSlider;
    public void OpenSettingsTab()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void Awake()
    {
        songSlider.value = PlayerPrefs.GetFloat("Song", 0.5f);
        soundSlider.value = PlayerPrefs.GetFloat("Sound", 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OpenSettingsTab();
    }

    public void SetSongVolume()
    {
        PlayerPrefs.SetFloat("Song", songSlider.value);
    }

    public void SetSoundVolume()
    {
        PlayerPrefs.SetFloat("Sound", soundSlider.value);
    }
}
