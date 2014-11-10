using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using Holoville.HOTween;

namespace WellFired
{
	public abstract class USHOTweenEventBase : USEventBase 
	{
		public string fieldName = "";
		
		public int targetIntValue = 0;
		public float targetFloatValue = 0.0f;
		public Color targetColorValue = Color.white;
		public Rect targetRectValue = new Rect();
		public Vector2 targetVec2Value = Vector2.zero;
		public Vector3 targetVec3Value = Vector3.zero;
		public Vector4 targetVec4Value = Vector4.zero;
		public Vector3 targetQuatValue = Vector3.zero;
		
		[SerializeField]
		private string targetTypeString = "vector3";
		private Type cachedType = null;
		
		[SerializeField]
		private string targetComponentString = "transform";
		private Component cachedComponent = null;
		
		protected Tweener tweener = null;
		public EaseType easeType = EaseType.Linear;
		
		public Type TargetType
		{
			get
			{
				if(cachedType != null)
					return cachedType;
				
				cachedType = ConvertTargetFromString(targetTypeString);
				return cachedType;
			}
			set
			{
				targetTypeString = ConvertTargetFromType(value);
				cachedType = ConvertTargetFromString(targetTypeString);
			}
		}
		
		public Component TargetComponent
		{
			get
			{
				if(cachedComponent != null)
					return cachedComponent;
				
				cachedComponent = AffectedObject.GetComponent(targetComponentString);
				return cachedComponent;
			}
			set
			{
				targetComponentString = value.GetType().Name;
				cachedComponent = AffectedObject.GetComponent(targetComponentString);
			}
		}
		
		public override void StopEvent()
		{	
			UndoEvent();
		}
		
		public override void UndoEvent()
		{	
			if(tweener == null)
				return;
			
			tweener.GoTo(0.0f);
			tweener.Pause();
			KillTweeners();
			
			if(Application.isEditor || !Application.isPlaying)
				return;
				
			tweener.Kill();
			tweener = null;
		}
		
		public override void PauseEvent()
		{
			if(tweener == null)
				return;
			
			tweener.Pause();
		}
		
		public override void ResumeEvent()
		{
			if(tweener == null)
				return;
			
			tweener.Play();
		}
		
		public override void EndEvent()
		{
			if(tweener == null)
				return;
			
			if(!Sequence.IsPingPonging)
			{
				KillTweeners();
				
				if(Application.isEditor || !Application.isPlaying)
					return;
	
				tweener.Kill();
				tweener = null;
			}
			else
			{
				tweener.GoTo(Duration);
				tweener.Pause();
			}
		}
		
		private void KillTweeners()
		{
			if(Application.isEditor && !Application.isPlaying)
			{
				HOTween[] tweens = Transform.FindObjectsOfType(typeof(HOTween)) as HOTween[];
				foreach(HOTween tween in tweens)
				{
					GameObject.DestroyImmediate(tween.gameObject);
				}
			}
		}
		
		protected Type ConvertTargetFromString(string type)
		{
			if(type == "int")
				return typeof(int);
			else if(type == "float")
				return typeof(float);
			else if(type == "color")
				return typeof(Color);
			else if(type == "rect")
				return typeof(Rect);
			else if(type == "vector2")
				return typeof(Vector2);
			else if(type == "vector3")
				return typeof(Vector3);
			else if(type == "vector4")
				return typeof(Vector4);
			else if(type == "quaternion")
				return typeof(Quaternion);
			
			return typeof(int);
		}
		
		protected string ConvertTargetFromType(Type type)
		{
			if(type == typeof(int))
				return "int";
			else if(type == typeof(float))
				return "float";
			else if(type == typeof(Color))
				return "color";
			else if(type == typeof(Rect))
				return "rect";
			else if(type == typeof(Vector2))
				return "vector2";
			else if(type == typeof(Vector3))
				return "vector3";
			else if(type == typeof(Vector4))
				return "vector4";
			else if(type == typeof(Quaternion))
				return "quaternion";
			
			return "int";
		}
		
		protected object GetTargetValue()
		{
			Type type = TargetType;
			
			if(type == typeof(int))
				return targetIntValue;
			else if(type == typeof(int))
				return targetFloatValue;
			else if(type == typeof(Color))
				return targetColorValue;
			else if(type == typeof(Rect))
				return targetRectValue;
			else if(type == typeof(Vector2))
				return targetVec2Value;
			else if(type == typeof(Vector3))
				return targetVec3Value;
			else if(type == typeof(Vector4))
				return targetVec4Value;
			else if(type == typeof(Quaternion))
				return targetQuatValue;
			
			return 0;
		}
	}
}