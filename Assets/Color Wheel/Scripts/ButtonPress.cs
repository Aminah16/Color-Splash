using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    public GameObject objectToSetActive;
    public GameObject objectToSetInactive;

    private void Start()
    {   
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
            objectToSetActive.SetActive(true);
            objectToSetInactive.SetActive(false);
    }
}