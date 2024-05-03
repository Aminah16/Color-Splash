using UnityEngine;

namespace EasyTransition
{

    public class DemoLoadScene : MonoBehaviour
    {
        public TransitionSettings transition;
        public float startDelay;

        TransitionManager manager;
        public void LoadScene(string _sceneName)
        {
            manager = TransitionManager.Instance();
            manager.Transition(transition, startDelay);
       }   
    }

}


