using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class MenuUI : MonoBehaviour
    {

        public RectTransform MenuBorder;

        public Level Level { get; set; }

        private bool isClosing;

        public void Close()
        {
            isClosing = true;
            MenuBorder.gameObject.GetComponent<Animator>().Play("MenuDisposing");
        }

        private void Update()
        {
            if (isClosing)
            {
                Destroy(gameObject);
            }
        }


        public void OnContinueButton()
        {
            Level.CloseMenu();
        }

        public void OnResetButton()
        {
            Level.CloseMenu(() => Level.Reset());
        }

        public void OnMainMenuButton()
        {
            
        }

    }
}
