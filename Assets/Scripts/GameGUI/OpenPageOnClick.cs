using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI
{
    public class OpenPageOnClick : MonoBehaviour
    {
        public Button Button;
        public GUIPageType PageType;

        private void Start()
        {
            if (Button == null)
                Button = GetComponent<Button>();

            if (Button == null)
            {
                Debug.LogWarning("OpenPageOnClick: Button not found");
                return;
            }

            Button.onClick.AddListener((wtfIsThisShit) => GUISupervisor.Instance.OpenPage(PageType)); // <- incorrect version


        }



    }
}
