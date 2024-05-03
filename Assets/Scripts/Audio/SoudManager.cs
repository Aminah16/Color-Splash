using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoudManager : MonoBehaviour
{
    [SerializeField] Image soundIcon;
    [SerializeField] Image soundIcon2nd;
    [SerializeField] Image soundOffIcon;
    [SerializeField] Image soundOffIcon2nd;
    private bool muted = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void OnButtonPress()
    {
        if (muted==false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
            muted=false;
            AudioListener.pause = false;
        }
        Save();
        UpdateButtonIcon();
        AudioListener.pause = muted;
    }

    private void UpdateButtonIcon()
    {
        if (muted== false)
        {
            soundIcon.enabled = true;
            soundIcon2nd.enabled = true;
            soundOffIcon.enabled = false;
            soundOffIcon2nd.enabled = false;
        }
        else
        {
            soundIcon.enabled = false;
            soundIcon2nd.enabled = false;
            soundOffIcon.enabled = true;
            soundOffIcon2nd.enabled = true;
        }
    }
    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
  
}
