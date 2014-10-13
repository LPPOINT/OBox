using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [ExecuteInEditMode]
    public class UIReferencePhysicsResolution : MonoBehaviour
    {

        private ReferenceResolution referenceResolution;

        private void Start()
        {
            referenceResolution = GetComponent<ReferenceResolution>();
        }

        private void Update()
        {
            referenceResolution.resolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        }
    }
}
