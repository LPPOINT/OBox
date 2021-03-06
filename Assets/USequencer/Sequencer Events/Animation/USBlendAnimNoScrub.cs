using UnityEngine;
using System.Collections;

namespace WellFired
{
	[USequencerFriendlyName("Blend Animation No Scrub (Legacy)")]
	[USequencerEvent("Animation (Legacy)/Blend Animation No Scrub")]
	public class USBlendAnimNoScrubEvent : USEventBase 
	{
		public AnimationClip blendedAnimation = null;
		
		public void Update() 
		{
			if(Duration < 0.0f)
				Duration = blendedAnimation.length;
		}
		
		public override void FireEvent()
		{	
			Animation animation = AffectedObject.GetComponent<Animation>();
			if(!animation)
			{
				Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
				return;
			}
			
	  	 	animation[blendedAnimation.name].wrapMode = WrapMode.Once;
			animation[blendedAnimation.name].layer = 1;
		}
		
		public override void ProcessEvent(float deltaTime)
		{
			animation.CrossFade(blendedAnimation.name);
		}
		
		public override void StopEvent()
		{
			if(!AffectedObject)
				return;
			
			Animation animation = AffectedObject.GetComponent<Animation>();
	        if (animation)
				animation.Stop();
		}
	}
}