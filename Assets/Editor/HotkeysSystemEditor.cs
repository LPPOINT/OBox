using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(HotkeysSystem))]
    public class HotkeysSystemEditor : UnityEditor.Editor
    {

        private HotkeysSystem.HotkeyAction newAction;

        private bool EditAction(HotkeysSystem.HotkeyAction action, bool canRemove)
        {
            EditorGUILayout.BeginHorizontal();

            if (canRemove && GUILayout.Button("Remove"))
            {
                return true;
            }

            action.Key = (KeyCode)EditorGUILayout.EnumPopup(action.Key);
            action.Object = EditorGUILayout.ObjectField(string.Empty, action.Object, typeof (GameObject), true) as GameObject;
            action.MethodName = EditorGUILayout.TextField(string.Empty, action.MethodName);


            EditorGUILayout.EndHorizontal();

            return false;
        }

        public override void OnInspectorGUI()
        {
            var hs = (HotkeysSystem) target;

            if(newAction == null) newAction = new HotkeysSystem.HotkeyAction();

            if (hs.Actions == null) hs.Actions = new List<HotkeysSystem.HotkeyAction>();

            HotkeysSystem.HotkeyAction deletedAction = null;

            foreach (var action in hs.Actions)
            {
                if (EditAction(action, true))
                {
                    deletedAction = action;
                    break;
                }
            }

            if (deletedAction != null)
            {
                hs.Actions.Remove(deletedAction);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditAction(newAction, false);

            if (GUILayout.Button("Add Hotkey"))
            {
                if (newAction.Object != null && !string.IsNullOrEmpty(newAction.MethodName))
                {
                    hs.Actions.Add(newAction);
                    newAction = new HotkeysSystem.HotkeyAction();
                }
            }

        }
    }
}
