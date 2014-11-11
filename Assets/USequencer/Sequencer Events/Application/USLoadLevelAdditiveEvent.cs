using UnityEngine;
using System.Collections;

namespace WellFired
{
	[USequencerFriendlyName("Load Level Additively")]
	[USequencerEvent("Application/Load Level Additive")]
	public class USLoadLevelAdditiveEvent : USEventBase 
	{
		public bool fireInEditor = false;
		
		public string levelName = "";
		public int levelIndex = -1;
		
		public override void FireEvent()
		{
			if(levelName.Length == 0 && levelIndex < 0)
			{
				Debug.LogError("You have a Load Level event in your sequence, however, you didn't give it a level to load.");
				return;
			}
			
			if(levelIndex >= Application.levelCount)
			{                                       
				Debug.LogError("You tried to load a level that is invalid, the level index is out of range.");
				return;
			}
			
			if(!Application.isPlaying && !fireInEditor)
			{                                                                                                             
			}
			
			if(levelName.Length != 0)
				Application.LoadLevelAdditive(levelName);
		
			if(levelIndex != -1)
				Application.LoadLevelAdditive(levelIndex);
		}
			
		public override void ProcessEvent(float deltaTime)
		{
	
		}
	}
}