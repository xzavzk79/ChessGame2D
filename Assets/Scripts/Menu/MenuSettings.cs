using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class MenuSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown rDd;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        rDd.ClearOptions();

        List<string> options = new List<string>();

        int cri = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                cri = i;
            }
        }

        rDd.AddOptions(options);
        rDd.value = cri;
        rDd.RefreshShownValue();
    }

    public void SetResolution(int ri)
    {
        Resolution resolution = resolutions[ri];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume (float vol)
    {
        audioMixer.SetFloat("vol", vol);
    }

    public void SetFullScreen(bool isFS)
    {
        Screen.fullScreen = isFS;
    }
}
