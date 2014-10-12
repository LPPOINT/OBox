using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Levels.Style.ElementsColorization
{
    public class RendererColored : ColoredObject
    {

#if UNITY_EDITOR
        public override IEnumerable<Object> GetDirtyObjects()
        {
            var r = new List<Object>();

            if(IncludeSelf && GetComponent<SpriteRenderer>() != null) r.Add(GetComponent<SpriteRenderer>());

            if (IncludeChildren)
            {
                r.AddRange(GetComponentsInChildren<SpriteRenderer>());
            }
            return r;
        }

#endif

        public override Color Color
        {
            get { return GetComponent<SpriteRenderer>().color; }
            set
            {
                var rr = GetComponent<SpriteRenderer>();

                if (rr != null) rr.color = value;

                if (IncludeChildren)
                {
                    foreach (var r in GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (!EnableAlphaMatching)
                        {
                            r.color = value;
                        }
                        else
                        {
                            r.color = new Color(value.r, value.g, value.b, r.color.a);
                        }
                    }
                }
            }
        }
    }
}
