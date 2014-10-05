using System.Xml;
using Assets.Scripts.Levels;
using Assets.Scripts.Meta.Model;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using LevelModel = Assets.Scripts.Levels.Model.LevelModel;

namespace Assets.Editor.Levels
{
    public class OpenLevelWindow : EditorWindow
    {
        [MenuItem("Levels/Open Level")]
        public static void Init()
        {
            var w = CreateInstance<OpenLevelWindow>();
            w.maxSize = new Vector2(750, 950);
            w.minSize = new Vector2(600, 800);
            w.Show();
        }



        private LevelModel currentModel;
        private Texture2D currentModelPreview;

        private void Awake()
        {

        }


        private void OnGUI()
        {
            var w = position.width;
            var h = position.height;

            const int offset = 10;
            var levelSelectWidth = w / 3 - offset;
            var levelSelectHeight = h - offset * 2;

            GUI.Box(new Rect(offset, offset, levelSelectWidth, levelSelectHeight), string.Empty);

            var currentH = offset + 10;
            var levels = LevelsDatabaseManager.GetDatabase().Levels;

            for (var i = 0; i < levels.Count; i++)
            {
                var model = levels[i];

                GUIStyle style;

                if (model == currentModel)
                {

                }

                if (GUI.Toggle(new Rect(offset + 10, currentH, levelSelectWidth, 20), currentModel == model,
                    "   Level" + model.LevelNumber + "-" + (int)model.WorldNumber))
                {
                    if (currentModel != model)
                    {
                        currentModel = model;
                        var pp = LevelPreviewManager.GetLevelPreviewPath(model.LevelNumber, model.WorldNumber, true);
                        currentModelPreview =
                            AssetDatabase.LoadAssetAtPath(
                                pp,
                                typeof(Texture2D)) as Texture2D;
                        if (currentModelPreview == null)
                        {
                            Debug.LogWarning("Preview for level not found");
                        }
                    }
                }

                currentH += 30;

            }

            if (currentModel != null)
            {
                var previewBoxOffset = 10;
                if (currentModelPreview != null)
                {
                    GUI.Box(
                        new Rect(
                            offset + levelSelectWidth + offset + (levelSelectWidth - currentModelPreview.width / 2) -
                            previewBoxOffset,
                            offset * 3 - previewBoxOffset,
                            currentModelPreview.width + previewBoxOffset * 2,
                            currentModelPreview.height + previewBoxOffset * 2), string.Empty);
                    GUI.DrawTexture(
                        new Rect(offset + levelSelectWidth + offset + (levelSelectWidth - currentModelPreview.width / 2),
                            offset * 3, currentModelPreview.width,
                            currentModelPreview.height), currentModelPreview);
                }

                var hh = (currentModelPreview != null)
                    ? offset * 3 - previewBoxOffset + currentModelPreview.height + previewBoxOffset * 2 + offset
                    : 20;
                var ww = (currentModelPreview != null) ? currentModelPreview.width / 2 :
                0;
                var www = (currentModelPreview != null) ? currentModelPreview.width : 0;

                if (
                    GUI.Button(
                        new Rect(
                            offset + levelSelectWidth + offset + (levelSelectWidth - ww) -
                            previewBoxOffset,
                            hh,
                            www + previewBoxOffset * 2,
                            30), "Open Level"))
                {
                    EditorApplication.OpenScene(currentModel.LevelPath);
                }

            }

        }
    }
}
