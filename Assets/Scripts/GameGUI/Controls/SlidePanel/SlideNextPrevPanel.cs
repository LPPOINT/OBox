using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Controls.SlidePanel
{
    public class SlideNextPrevPanel : MonoBehaviour
    {
        public Button Next;
        public Button Prev;
        public SlidePanel SlidePanel;

        private bool IsInvalid()
        {
            return Next == null || Prev == null;
        }

        private void CheckVisibility()
        {
            if (SlidePanel.ActiveNodeIndex == 0)
            {
                Prev.gameObject.SetActive(false);
            }
            else Prev.gameObject.SetActive(true);

            if (SlidePanel.ActiveNodeIndex >= SlidePanel.Nodes.Count - 1)
            {
                Next.gameObject.SetActive(false);
            }
            else Next.gameObject.SetActive(true);
        }

        private void Start()
        {
            if (IsInvalid())
            {
                Debug.LogWarning("SlideNextPrevPanel not initialized in inspector");
                return;
            }

            Next.onClick.AddListener(() =>
                                     {
                                         SlidePanel.Next();
                                         CheckVisibility();
                                     });
            Prev.onClick.AddListener(() =>
                                     {
                                         SlidePanel.Prev();
                                         CheckVisibility();
                                     });

            CheckVisibility();

        }

    }
}
