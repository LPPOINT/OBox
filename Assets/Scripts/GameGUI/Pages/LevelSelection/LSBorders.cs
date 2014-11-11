using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{

    [ExecuteInEditMode]
    public class LSBorders : MonoBehaviour
    {

        public static LSBorders Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public Transform Min;
        public Transform Max;
    }
}
