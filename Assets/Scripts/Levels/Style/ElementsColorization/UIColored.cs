using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Levels.Style.ElementsColorization
{
    public class UIColored : ColoredObject
    {


        public override IEnumerable<Object> GetDirtyObjects()
        {
            var r = new List<Object>();

            if (IncludeSelf && GetComponent<Graphic>() != null) r.Add(GetComponent<Graphic>());

            if (IncludeChildren)
            {
                r.AddRange(GetComponentsInChildren<Graphic>());
            }
            return r;
        }


        public override Color Color
        {
            get
            {
                return GetComponent<Graphic>().color;
            }
            set
            {

                if (IncludeSelf)
                {
                    var gg = GetComponent<Graphic>();

                    if (gg != null) gg.color = value;
                }

                if (IncludeChildren)
                {
                    foreach (var c in GetComponentsInChildren<Graphic>())
                    {
                        if (!EnableAlphaMatching)
                        {
                            c.color = value;
                        }
                        else
                        {
                            c.color = new Color(value.r, value.g, value.b, c.color.a);
                        }
                    }
                }
            }
        }
    }
}
