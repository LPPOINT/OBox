using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Utils
{
    public class AutoSave : EditorWindow
    {
        public string relativeSavePath = "Assets/";
        public string autoSaveFileName = "AutoSave";
        public int secondsBetweenSaves = 600;

        [MenuItem("Edit/Project Settings/AutoSave")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            AutoSave window = (AutoSave)EditorWindow.GetWindow(typeof(AutoSave));
            window.minSize = new Vector2(225, 21);
            window.Show();
        }

        float nextsave = 0;
        int timeLeft = 0;
        bool showDisclaimer = true;
        bool showDebug = true;
        bool toggleVisible = false;
        Vector2 winPosition = Vector2.zero;
        Vector2 disPosition = Vector2.zero;

        void OnGUI()
        {
            if (GUILayout.Button("Created By ReverieInteractive"))
            {
                Application.OpenURL("www.reverieinteractive.com");
            }
            GUI.color = Color.white;
            GUI.contentColor = Color.white;
            winPosition = GUILayout.BeginScrollView(winPosition);
            if (showDisclaimer = EditorGUILayout.Foldout(showDisclaimer, "Disclaimer and Information"))
            {
                GUI.contentColor = Color.yellow;
                disPosition = GUILayout.BeginScrollView(disPosition);
                GUILayout.TextArea("IMPORTANT: The autosave will only save scenes if this window is OPEN and VISIBLE. If you hide this window, the autosave WILL NOT WORK!\n\n-------HELP INFO------\n\n\"Save Iteration\" - Seconds between saves (600 = saves every 10 min)\n\n\"File Name\" - The name of the AutoSave file (DONT name this the same as your opened scene)\n\n\"Relative Path\" - The path (from your projects folder up) to where the file is saved\n\n\"Apply Instantly\" - Applies settings and saves the scene without having to wait for the next save cycle");
                GUILayout.EndScrollView();
            }
            GUI.contentColor = Color.white;
            if (toggleVisible = EditorGUILayout.Foldout(toggleVisible, "AutoSave Settings"))
            {
                secondsBetweenSaves = EditorGUILayout.IntField("Save Iteration", secondsBetweenSaves);
                autoSaveFileName = EditorGUILayout.TextField("File Name", autoSaveFileName);
                relativeSavePath = EditorGUILayout.TextField("Relative Path", relativeSavePath);
                showDebug = EditorGUILayout.Toggle("Show Warning", showDebug);
            }

            if (GUILayout.Button("Apply Instantly"))
            {
                SaveScene();
            }
            EditorGUILayout.LabelField("Next Save:", timeLeft + " Seconds");
            GUILayout.EndScrollView();
        }

        void OnInspectorUpdate()
        {
            timeLeft = (int)(nextsave - Time.realtimeSinceStartup);
            this.Repaint();
        }

        void Update()
        {
            if (Time.realtimeSinceStartup >= nextsave)
            {
                SaveScene();
            }
        }

        void SaveScene()
        {
            EditorApplication.SaveScene(relativeSavePath + autoSaveFileName + ".unity");
            nextsave = Time.realtimeSinceStartup + secondsBetweenSaves;

            if (showDebug)
                Debug.LogWarning("Saved scene: " + relativeSavePath + autoSaveFileName + ".unity");
        }
    }
}