using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Holoville.HOTween;

namespace WellFired
{
	[CustomEditor(typeof(USHOTweenFrom))]
	public class USHOTweenFromEventEditor : USEventBaseEditor
	{
		public override void OnInspectorGUI()
		{
			USUndoManager.BeginChangeCheck();
			
			serializedObject.Update();
			
			USHOTweenEventBase baseTweenEvent = serializedObject.targetObject as USHOTweenEventBase;
			if(!baseTweenEvent.AffectedObject)
				return;
			
			baseTweenEvent.Duration = EditorGUILayout.FloatField("Duration", baseTweenEvent.Duration);
			if(baseTweenEvent.Duration < 0.0f)
				baseTweenEvent.Duration = 1.0f;
			baseTweenEvent.Duration = Mathf.Clamp(baseTweenEvent.Duration, 0, float.MaxValue);
			
			baseTweenEvent.easeType = (EaseType)EditorGUILayout.EnumPopup("Ease Type", baseTweenEvent.easeType);
			
			string[] componentNames = baseTweenEvent.AffectedObject.GetComponents<Component>().Select(c => c.GetType().Name).ToArray();
			int componentIndex = baseTweenEvent.TargetComponent != null ? componentNames.ToList().FindIndex(n => n == baseTweenEvent.TargetComponent.GetType().Name) : 0;
			if(componentIndex == -1)
				componentIndex = 0;
			
			componentIndex = EditorGUILayout.Popup(componentIndex, componentNames);
			baseTweenEvent.TargetComponent = baseTweenEvent.AffectedObject.GetComponent(componentNames[componentIndex]);
			
			Component selectedComponent = baseTweenEvent.TargetComponent;
			PropertyInfo[] properties = selectedComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			FieldInfo[] fields = selectedComponent.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			
			string[] propertyNames = properties.Where(p => IsValidType(p.PropertyType)).Select(p => p.Name).ToArray();
			string[] fieldNames = fields.Where(f => IsValidType(f.FieldType)).Select(f => f.Name).ToArray();
			string[] allNames = propertyNames.Concat(fieldNames).ToArray();
			
			int propertyIndex = allNames.ToList().FindIndex(n => n == baseTweenEvent.fieldName);
			if(propertyIndex == -1)
				propertyIndex = 0;
			
			propertyIndex = EditorGUILayout.Popup(propertyIndex, allNames);
			baseTweenEvent.fieldName = allNames[propertyIndex];
			
			PropertyInfo property = properties.Count() > 0 ? properties.Single(p => p.Name == baseTweenEvent.fieldName) : null;
			FieldInfo field = fields.Count() > 0 ? fields.Single(f => f.Name == baseTweenEvent.fieldName) : null;
			
			Type propertyType = null;
			if(property != null)
				propertyType = property.PropertyType;
			if(field != null)
				propertyType = field.FieldType;
			
			if(propertyType == null)
				return;
			
			baseTweenEvent.TargetType = propertyType;
			
			DisplayCorrectGUIElement(baseTweenEvent);
	
			if(USUndoManager.EndChangeCheck())
			{
				USUndoManager.PropertyChange(serializedObject.targetObject, "Inspector");
	
				USWindow[] windows = Resources.FindObjectsOfTypeAll<USWindow>();
				foreach(var window in windows)
					window.ExternalModification();
			}
			
			serializedObject.ApplyModifiedProperties();
		}
		
		private void DisplayCorrectGUIElement(USHOTweenEventBase baseTweenEvent)
		{
			Type type = baseTweenEvent.TargetType;
			
			if(type == typeof(int))
				baseTweenEvent.targetIntValue = EditorGUILayout.IntField("value", baseTweenEvent.targetIntValue);
			else if(type == typeof(float))
				baseTweenEvent.targetFloatValue = EditorGUILayout.FloatField("value", baseTweenEvent.targetFloatValue);
			else if(type == typeof(Color))
				baseTweenEvent.targetColorValue = EditorGUILayout.ColorField("value", baseTweenEvent.targetColorValue);
			else if(type == typeof(Rect))
				baseTweenEvent.targetRectValue = EditorGUILayout.RectField("value", baseTweenEvent.targetRectValue);
			else if(type == typeof(Vector2))
				baseTweenEvent.targetVec2Value = EditorGUILayout.Vector2Field("value", baseTweenEvent.targetVec2Value);
			else if(type == typeof(Vector3))
				baseTweenEvent.targetVec3Value = EditorGUILayout.Vector3Field("value", baseTweenEvent.targetVec3Value);
			else if(type == typeof(Vector4))
				baseTweenEvent.targetVec4Value = EditorGUILayout.Vector4Field("value", baseTweenEvent.targetVec4Value);
			else if(type == typeof(Quaternion))
				baseTweenEvent.targetQuatValue = EditorGUILayout.Vector4Field("value", baseTweenEvent.targetQuatValue);
		}
		
		private bool IsValidType(Type type)
		{
			if(type == typeof(int) || type == typeof(float) || type == typeof(Color) || type == typeof(Rect) || 
				type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Vector4))
				return true;
			
			return false;
		}
	}
}