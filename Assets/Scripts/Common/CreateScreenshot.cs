using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class CreateScreenshot : MonoBehaviour
    {

        public void Create()
        {

            StartCoroutine(WaitAndCreate());


        }

        private IEnumerator WaitAndCreate()
        {

            yield return new WaitForEndOfFrame();



            // Create a texture the size of the screen, RGB24 format
            var width = Screen.width;
            var height = Screen.height;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            // Encode texture into PNG
            var bytes = tex.EncodeToPNG();

            using (var f = File.Create(@"C:\Users\Sasha\Documents\OBox\Assets\Editor\Screenshots\Scr.png"))
            {
                f.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
