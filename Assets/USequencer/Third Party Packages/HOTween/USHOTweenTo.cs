using UnityEngine;
using System.Collections;
using Holoville.HOTween;

namespace WellFired
{
	[USequencerFriendlyName("Tween To")]
	[USequencerEvent("HOTween/Tween To")]
	public class USHOTweenTo : USHOTweenEventBase 
	{	
		public override void FireEvent()
		{
			if(tweener != null)
				return;
			
			TweenParms parms = new TweenParms();
			
			parms.Prop(fieldName, GetTargetValue());
			parms.Ease(easeType);
			parms.AutoKill(false);
			
			tweener = HOTween.To(TargetComponent, Duration, parms);
		}
		
		public override void ProcessEvent(float deltaTime)
		{
			if(tweener == null)
				FireEvent();
			
			tweener.GoTo(deltaTime);
		}
	}
}