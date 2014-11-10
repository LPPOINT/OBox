using UnityEngine;
using System.Collections;
using Holoville.HOTween;

namespace WellFired
{
	[USequencerFriendlyName("Tween From")]
	[USequencerEvent("HOTween/Tween From")]
	public class USHOTweenFrom : USHOTweenEventBase 
	{	
		public override void FireEvent()
		{
			if(tweener != null)
				return;
			
			TweenParms parms = new TweenParms();
			
			parms.Prop(fieldName, GetTargetValue());
			parms.Ease(easeType);
			parms.AutoKill(false);
			
			tweener = HOTween.From(TargetComponent, Duration, parms);
		}
		
		public override void ProcessEvent(float deltaTime)
		{
			if(tweener == null)
				FireEvent();
			
			tweener.GoTo(deltaTime);
		}
	}
}