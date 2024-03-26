using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject objectToSetActive;
    public GameObject objectToSetInactive;  
    public AudioSource clip;
    public Button button;
    public Button selected;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        selected.Select();
      
    }

    public void OnButtonClick()
    {
        objectToSetActive.SetActive(true);
        objectToSetInactive.SetActive(false);   
        clip.Play();
    }
}
