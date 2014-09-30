using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.Scripts.Map.Collision;
using Assets.Scripts.Map.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Map.Items
{
    public class Wall : MapItem
    {


        private Vector3 startScale;


        protected override void Start()
        {
            base.Start();
            startScale = transform.localScale;
        }

        public override MapItemColliderType GetCollider(MapItem other)
        {
            if (transform.localScale != Vector3.zero)
            {
                return MapItemColliderType.StopNear;
            }
            return MapItemColliderType.MoveThrow;
        }

        public override void OnDebugClick(IEnumerable<DebugClickModifers> modifers)
        {
            base.OnDebugClick( modifers);

            if (modifers.Contains(DebugClickModifers.Ctrl))
            {
                if (transform.localScale != Vector3.zero)
                {
                    transform.localScale = Vector3.zero;
                }
                else
                {
                    transform.localScale = startScale;
                }
            }
        }

        public override void OnLevelReset()
        {
            if (GetType() != typeof (Wall))
            {
                base.OnLevelReset();
            }
            transform.localScale = startScale;
        }
    }
}
