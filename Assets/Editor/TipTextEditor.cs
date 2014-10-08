using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Map;
using Assets.Scripts.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor
{
    [CustomEditor(typeof(TipTextManager))]
    public class TipTextEditor : UnityEditor.Editor
    {


        private static IEnumerable<Text> GetAllTextsOfCanvas(Canvas canvas)
        {
            return canvas.GetComponentsInChildren<Text>();
        }

        private string newTipText = string.Empty;

        private GameObject GetTipPrefab()
        {
            var p = AssetDatabase.LoadAssetAtPath(@"Assets/Prefabs/UI/TipTextManager.prefab", typeof(GameObject));
            if (p == null)
            {
                Debug.LogWarning("TipTextEditor(): GetTipPrefab(): prefab == null");
                return null;
            }
            return p as GameObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var canvas = (target as Component).gameObject.GetComponent<Canvas>();
            var tips = GetAllTextsOfCanvas(canvas).ToList();
            GUILayout.Space(10);

            if (GUILayout.Button("Highlight tip prefab"))
            {
                EditorGUIUtility.PingObject(GetTipPrefab());
            }
            if (tips.Any() && GUILayout.Button("Delete all tips"))
            {
                foreach (var text in tips)
                {
                    DestroyImmediate(text);
                }
            }
            else
            {
                GUILayout.Space(10);


                for (var i = 0; i < tips.Count(); i++)
                {
                    var tip = tips[i];

                    var newText = EditorGUILayout.TextField("Tip #" + (i + 1) + ": ", tip.text);
                    if (newText != tip.text)
                    {
                        tip.text = newText;
                        tip.SetAllDirty();
                    }

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Delete tip"))
                    {
                        DestroyImmediate(tip.gameObject);
                    }
                    if (GUILayout.Button("Highlight tip"))
                    {
                        EditorGUIUtility.PingObject(tip);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                }
            }

            GUILayout.Space(10);

            newTipText = EditorGUILayout.TextField("New tip text: ", newTipText);
            if (GUILayout.Button("Add tip"))
            {

                var tipPrefab = GetTipPrefab();
                var tip = Instantiate(tipPrefab, tipPrefab.transform.position, tipPrefab.transform.rotation) as GameObject;

                tip.transform.localScale = tipPrefab.transform.localScale;

                tip.GetComponent<RectTransform>().position = tipPrefab.GetComponent<RectTransform>().position;
                tip.GetComponent<RectTransform>().localScale = tipPrefab.GetComponent<RectTransform>().localScale;
               

                if (tip != null)
                {

                    tip.transform.parent = canvas.gameObject.transform;
                    tip.GetComponent<Text>().text = newTipText;

                    newTipText = string.Empty;
                }
            }


        }
    }
}
