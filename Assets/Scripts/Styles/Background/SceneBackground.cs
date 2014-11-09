﻿using Assets.Scripts.Levels;
﻿using UnityEngine;

namespace Assets.Scripts.Styles.Background
{
    public class SceneBackground : MonoBehaviour
    {

        public static SceneBackground Main { get; private set; }

        private void Awake()
        {
            if(gameObject.transform.parent == null || gameObject.transform.parent.GetComponent<SceneBackground>() == null)
                Main = this;
        }

        public void AlignToBackAnchor()
        {
            if (LevelDepth.IsExist)
                LevelDepth.AlignToBack(transform);
        }
        public void AlignToFrontAnchor()
        {
            if (LevelDepth.IsExist)
                LevelDepth.AlignToFront(transform);
        }

        public SceneBackground StoredCopy { get; private set; }

        public void Store()
        {
            StoredCopy = (Instantiate(gameObject) as GameObject).GetComponent<SceneBackground>();
            StoredCopy.gameObject.transform.parent = gameObject.transform;
            StoredCopy.gameObject.SetActive(false);


        }

        public void Restore()
        {
            StoredCopy.gameObject.transform.parent = null;
            StoredCopy.gameObject.SetActive(true);
            Destroy(gameObject);
        }



    }
}