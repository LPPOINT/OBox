using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holoville.HOTween;
using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class GotoTest : MonoBehaviour
    {
        public float current = 0;
        public float step = 0.06f;

        public GameObject Target;
        private List<IHOTweenComponent> tweeners; 
        
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            tweeners = new List<IHOTweenComponent>(HOTween.GetAllTweens());
            foreach (var tweener in tweeners)
            {
                tweener.Pause();
            }
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                current += step;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                current -= step;
            }
            else return;

            foreach (var t  in tweeners)
            {
                t.GoTo(current);
            }

        }
    }
}
