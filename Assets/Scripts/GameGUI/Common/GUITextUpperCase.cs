using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Common
{
    public class GUITextUpperCase : MonoBehaviour
    {
        private IEnumerator Start()
        {
            var t = GetComponent<Text>();
            yield return new WaitForEndOfFrame();
            t.text = t.text.ToUpper();
        }
    }
}
