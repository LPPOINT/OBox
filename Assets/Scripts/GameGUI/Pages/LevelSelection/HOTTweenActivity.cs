using System.Collections.Generic;
using System.Linq;
using Holoville.HOTween;
using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class HOTTweenActivity : LSActivity
    {
        
        public string Id;

        private List<IHOTweenComponent> tweens;

        private void Start()
        {
            tweens = HOTween.GetTweensById(Id, true);
        }

        protected override void OnActivityUpdate(float seed)
        {

         
            foreach (var t in tweens)
            {
                var duration = t.fullDuration;
                var time = duration*seed/Lenght;
                t.GoTo(time);
            }

        }
    }
}
