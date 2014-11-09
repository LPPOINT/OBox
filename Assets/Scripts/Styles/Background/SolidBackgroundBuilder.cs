using UnityEngine;

namespace Assets.Scripts.Styles.Background
{
    public class SolidBackgroundBuilder : SceneBackgroundBuilder
    {
        public SolidBackgroundBuilder(Color color)
        {
            Color = color;
        }

        public Color Color { get; private set; }

        public override void Build(SceneBackground background)
        {
            Debug.Log("Building bg");
            var sr = background.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color;
            }
            else
            {
                var mr = background.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    Debug.Log("Mesh found");
                    mr.material.color = Color;
                }
                else Debug.Log("Mesh not found");
            }
        }
    }
}
