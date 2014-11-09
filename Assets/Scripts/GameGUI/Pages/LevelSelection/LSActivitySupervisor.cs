using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class LSActivitySupervisor : MonoBehaviour
    {
        public static LSActivitySupervisor Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            activities = new List<LSActivity>(FindObjectsOfType<LSActivity>());
        }

        private  List<LSActivity> activities = new List<LSActivity>(); 

        private void RegisterActivityBecameVisible(LSActivity activity)
        {
            activity.OnActivityBecameActivated();
        }

        private void RegisterActivityBecameInvisible(LSActivity activity)
        {
            activity.OnActivityBecameDeactivated();
        }

        public void RegisterCameraMove()
        {
            var thisCam = UnityEngine.Camera.main;
            var farClip = thisCam.farClipPlane;


            var topLeftPosition = thisCam.ViewportToWorldPoint(new Vector3(0, 1, farClip));
            var topRightPosition = thisCam.ViewportToWorldPoint(new Vector3(1, 1, farClip));

            var minX = topLeftPosition.x;
            var maxX = topRightPosition.x;

            foreach (var a in activities)
            {

                var aMinX = a.ActivityStartPosition.position.x;
                var aMaxX = a.ActivityEndPosition.position.x;

                if (a.IsActive)
                {
                   
                    if (minX > aMaxX || maxX < aMinX)
                    {
                        a.OnActivityBecameDeactivated();
                    }
                    var seed = maxX -
                        aMinX;
                    a.ReceiveUpdate(seed);
                }
                else
                {
                    if ((maxX > aMinX && maxX < aMaxX) || (minX < aMaxX && minX > aMinX))
                    {
                        a.OnActivityBecameActivated();
                    }
                }
            }
        }

        private void Update()
        {
            
        }

    }
}
