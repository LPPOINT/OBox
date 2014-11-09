using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Common
{
    public class PlaySoundOnClick : MonoBehaviour
    {
        public Button Button;
        public AudioClip Audio;

        private void Start()
        {
            if (Button == null)
                Button = GetComponent<Button>();

            if (Button == null)
            {
                Debug.LogWarning("OpenPageOnClick: Button not found");
                return;
            }

            Button.onClick.AddListener(() => AudioSource.PlayClipAtPoint(Audio, Button.transform.position)); // <- incorrect version


        }
    }
}
