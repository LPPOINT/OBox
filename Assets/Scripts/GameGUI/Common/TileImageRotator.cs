using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Common
{
    public class TileImageRotator : MonoBehaviour
    {

        public Image TileImage;

        public void Rotate()
        {
            if (TileImage == null)
                TileImage = GetComponent<Image>();
            iTween.RotateBy(TileImage.gameObject, new Vector3(0, 0, TileImage.gameObject.transform.rotation.eulerAngles.z + 90), 0.5f);
        }
    }
}
