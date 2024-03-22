using UnityEngine;
using UnityEngine.UI;
public class ButtonSelect : MonoBehaviour
{

    public AudioSource clip;
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    public void OnButtonClick()
    {
        clip.Play();
    }
}
