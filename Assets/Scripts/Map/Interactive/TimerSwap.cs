using UnityEngine;

namespace Assets.Scripts.Map.Interactive
{
    public class TimerSwap : MonoBehaviour
    {
        public GameObject Timer;
        public GameObject End;
        public GameObject Movable;

        public AudioSource TranslationAudio;

        private void Start()
        {
            Timer.GetComponent<InfinityTimer>().TimerDone += (sender, args) => Swap();
        }
        public void Swap()
        {
            var offset = End.transform.position.x - Timer.transform.position.x;

            iTween.MoveTo(Movable, iTween.Hash("position", new Vector3(Movable.transform.position.x - offset, Movable.transform.position.y, Movable.transform.position.z), "time", 0.2f, "easetype", iTween.EaseType.easeInBack));
            if(TranslationAudio != null) TranslationAudio.Play();
        }

    }
}
