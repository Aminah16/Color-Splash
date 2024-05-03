using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    public GameObject objectToSetActive;
    public GameObject objectToSetInactive;
    public AudioSource clip;
    public Button button;




    private void Start()
    {
       
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
 
  

    public void OnButtonClick()
    {
            clip.Play();
        StartCoroutine(Delayed());
    }
    private IEnumerator Delayed()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        objectToSetActive.SetActive(true);
        objectToSetInactive.SetActive(false);
    }

}
