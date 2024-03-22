using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject objectToSetActive;
    public GameObject objectToSetInactive;  
    public AudioSource clip;
    public Button button;
    public Button selected;
    public Toggle toggle;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        selected.Select();
        toggle.isOn = true;
        toggle.Select();
        
    }

    public void OnButtonClick()
    {
        objectToSetActive.SetActive(true);
        objectToSetInactive.SetActive(false);   
        clip.Play();
    }
}
