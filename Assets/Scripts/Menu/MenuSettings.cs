using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    public float volume = 0;
    public bool isFullScreen = false;
    public AudioMixer audioMixer;
    public Dropdown resolutionDropDown;
    private Resolution[] resolutions;
    private int currResolutionIndex = 0;


    public void ChangeVolume(float val)
    {
        volume = val;
    }
    public void ChangeResolution(int index)
    {
        currResolutionIndex = index;
    }
    public void ChangeFullScreen(bool val)
    {
        isFullScreen = val;
    }
    public void SaveSettings()
    {
        audioMixer.SetFloat("MasterVolume", volume);
        Screen.fullScreen = isFullScreen;
        Screen.SetResolution(Screen.resolutions[currResolutionIndex].width, Screen.resolutions[currResolutionIndex].height, isFullScreen);
    }
}
