using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Scripts.Ads
{
    public class AdsStarter : MonoBehaviour
    {
        private void Start()
        {
            Advertisement.Initialize(Ads.GameId);
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Advertisement.isReady())
                {
                    Advertisement.Show();
                }
            }
        }

    }
}
