using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WellFired
{
    [USequencerFriendlyName("TEST")]
    [USequencerEvent("Debug/TEST")]
    public class TestEvent : USEventBase 
    {
        public override void FireEvent()
        {
            Debug.Log("FireEvent");
            
        }



        public override void ProcessEvent(float runningTime)
        {
            Debug.Log("ProcesssEvent");
        }
    }
}
