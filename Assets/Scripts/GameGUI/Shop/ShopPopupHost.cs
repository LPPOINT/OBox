using System.Collections;
using Assets.Scripts.Camera.Effects;
using UnityEngine;

namespace Assets.Scripts.GameGUI.Shop
{
    public class ShopPopupHost : MonoBehaviour
    {

        public static ShopPopupHost Instance { get; private set; }
        public static bool IsExist { get { return Instance != null; } }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }


        public ShopPopup Prefab;

        public void Open()
        {
            if (Prefab == null)
            {
                Debug.LogWarning("ShopPopupHost.Open(): shop prefab not found");
                return;
            }

            StartCoroutine(PerformOpen());

        }

        private IEnumerator PerformOpen()
        {
            CameraBlurEffect.BlurIn();
            yield return new WaitForSeconds(0.3f);
            var shop = (Instantiate(Prefab.gameObject) as GameObject).GetComponent<ShopPopup>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if(ShopPopup.IsOpened)
                    ShopPopup.Current.Close();
                else Open();
            }
        }

    }
}
