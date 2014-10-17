using UnityEngine;

namespace Assets.Scripts.LevelSelection
{
    public class LSNode : MonoBehaviour
    {
        public LSTrigger PrevNodeLoadTrigger;
        public LSTrigger NextNodeLoadTrigger;

        public Transform BoundMin;
        public Transform BoundsMax;
        public void DestroyEditorCamera()
        {
            var editorCameras = GameObject.FindGameObjectsWithTag("EditorCamera");

            foreach (var c in editorCameras)
            {
                Destroy(c);
            }
        }


    }
}
