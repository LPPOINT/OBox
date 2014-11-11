using Assets.Scripts.GameGUI.Pages.LevelSelection;
using Assets.Scripts.GameGUI.Pages.LevelSelection.USequencerIntegration;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.LevelSelection
{
    public class LevelSelectionOverviewWindow : EditorWindow
    {


        [MenuItem("Window/Level Selection Overview")]
        private static void Init()
        {
            var w = GetWindow<LevelSelectionOverviewWindow>();
            w.title = "LS Overview";
            w.autoRepaintOnSceneChange = true;

            w.Show();
        }

        public bool DrawCameraBorders = true;
        public Color CameraBordersColor = Color.black;
        public float CameraBordersWidth = 1;

        public bool DrawSequenceTimelines = true;

        public bool DrawCameraCenterLine = true;
        public Color CameraCenterLineColor = Color.red;
        public float CameraCenterLineWidth = 0.22f;
        public float CameraCenterLineHeight = 30;

        public bool DrawHorizontalMarkers = true;
        public Color HorizontalMarkersColor = Color.black;
        public float HorizontalMarkersWidth = 0.5f;
        public float HorizontalMarkersLenght = 300;


        private void DrawSimpleLineWithWidth(Vector2 start, Vector2 end, Color color, float width)
        {
            var oldColor = Handles.color;
            Handles.color = color;
            
            //Handles.DrawSolidRectangleWithOutline(new Vector3[]
            //            {
            //                new Vector3(), 
            //            },
            //            color,
            //            color
            //    );
            Handles.DrawLine(start, end);

            Handles.color = oldColor;
        }

        private void DrawBordersForCamera(Camera thisCam)
        {
            var farClip = thisCam.farClipPlane;

            var topLeftPosition = thisCam.ViewportToWorldPoint(new Vector3(0, 1, farClip));
            var topRightPosition = thisCam.ViewportToWorldPoint(new Vector3(1, 1, farClip));
            var btmRightPosition = thisCam.ViewportToWorldPoint(new Vector3(1, 0, farClip));
            var btmLeftPosition = thisCam.ViewportToWorldPoint(new Vector3(0, 0, farClip));

            DrawSimpleLineWithWidth(topLeftPosition, topRightPosition, CameraBordersColor, CameraBordersWidth);
            DrawSimpleLineWithWidth(topRightPosition, btmRightPosition, CameraBordersColor, CameraBordersWidth);
            DrawSimpleLineWithWidth(btmRightPosition, btmLeftPosition, CameraBordersColor, CameraBordersWidth);
            DrawSimpleLineWithWidth(btmLeftPosition, topLeftPosition, CameraBordersColor, CameraBordersWidth);
        }

        private void DrawMarkerForCamera(Camera thisCam)
        {
            var pos = thisCam.transform.position;

            Handles.color = CameraCenterLineColor;
            Handles.DrawSolidRectangleWithOutline(new Vector3[]
                                                  {
                                                      new Vector3(pos.x - CameraCenterLineWidth/2, pos.y + CameraCenterLineHeight/2),
                                                      new Vector3(pos.x + CameraCenterLineWidth/2, pos.y + CameraCenterLineHeight/2), 
                                                      new Vector3(pos.x + CameraCenterLineWidth/2, pos.y - CameraCenterLineHeight/2),
                                                      new Vector3(pos.x - CameraCenterLineWidth/2, pos.y - CameraCenterLineHeight/2), 
                                                  },
                                                  CameraCenterLineColor,
                                                  CameraCenterLineColor);
        }

        private void MoveToCamera(Camera camera)
        {
            if (camera == null) return;
            SceneView.currentDrawingSceneView.LookAt(camera.transform.position, Quaternion.identity, camera.orthographicSize + camera.orthographicSize/3);

        }

        private void OnGUI()
        {
            DrawInspector();

        }

        private void OnDestroy()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        private void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        private void DrawInspector()
        {
            var boldtext = new GUIStyle(GUI.skin.label);
            boldtext.fontStyle = FontStyle.Bold;
            var space = 10;

            GUILayout.Space(space);
            EditorGUILayout.LabelField("Camera Borders", boldtext);
            DrawCameraBorders = EditorGUILayout.Toggle("Enabled", DrawCameraBorders);
            CameraBordersColor = EditorGUILayout.ColorField("Color", CameraBordersColor);
            CameraBordersWidth = EditorGUILayout.FloatField("Width", CameraBordersWidth);
            EditorGUILayout.Separator();
            GUILayout.Space(space);

            EditorGUILayout.LabelField("Sequence timelines", boldtext);
            DrawSequenceTimelines = EditorGUILayout.Toggle("Enabled", DrawSequenceTimelines);
            EditorGUILayout.Separator();
            GUILayout.Space(space);

            EditorGUILayout.LabelField("Camera Marker", boldtext);
            DrawCameraCenterLine = EditorGUILayout.Toggle("Enabled", DrawCameraCenterLine);
            CameraCenterLineColor = EditorGUILayout.ColorField("Color", CameraCenterLineColor);
            CameraCenterLineWidth = EditorGUILayout.FloatField("Width", CameraCenterLineWidth);
            CameraCenterLineHeight = EditorGUILayout.FloatField("Height", CameraCenterLineHeight);
            EditorGUILayout.Separator();
            GUILayout.Space(space);

            EditorGUILayout.LabelField("Screen Bounds Marker", boldtext);
            DrawHorizontalMarkers = EditorGUILayout.Toggle("Enabled", DrawHorizontalMarkers);
            HorizontalMarkersColor = EditorGUILayout.ColorField("Color", HorizontalMarkersColor);
            HorizontalMarkersWidth = EditorGUILayout.FloatField("Width", HorizontalMarkersWidth);
            HorizontalMarkersLenght = EditorGUILayout.FloatField("Lenght", HorizontalMarkersLenght);
            EditorGUILayout.Separator();
            GUILayout.Space(space);

            if (GUILayout.Button("Move to DynamicCamera"))
            {
                MoveToCamera(Camera.main);
            }
            else if (GUILayout.Button("Move to StaticCamera"))
            {
                MoveToCamera(GameObject.FindGameObjectWithTag("StaticCamera").camera);
            }
        }

        private void OnSceneGUI(SceneView scene)
        {
            var oldColor = Handles.color;
            if (DrawCameraBorders)
            {
                DrawBordersForCamera(Camera.main);

                var staticCamera = GameObject.FindGameObjectWithTag("StaticCamera");
                if(staticCamera != null) 
                    DrawBordersForCamera(staticCamera.camera);

            }
            if (DrawCameraCenterLine)
            {
                DrawMarkerForCamera(Camera.main);
            }
            if (DrawHorizontalMarkers)
            {
                var thisCam = Camera.main;
                var farClip = thisCam.farClipPlane;

                var topLeftPosition = thisCam.ViewportToWorldPoint(new Vector3(0, 1, farClip));
                var btmLeftPosition = thisCam.ViewportToWorldPoint(new Vector3(0, 0, farClip));

                var borders = LSBorders.Instance ?? FindObjectOfType<LSBorders>();

                DrawSimpleLineWithWidth(new Vector2(borders.Min.transform.position.x, btmLeftPosition.y), new Vector2(borders.Max.transform.position.x, btmLeftPosition.y), HorizontalMarkersColor, HorizontalMarkersWidth);
                DrawSimpleLineWithWidth(new Vector2(borders.Min.transform.position.x, topLeftPosition.y), new Vector2(borders.Max.transform.position.x, topLeftPosition.y), HorizontalMarkersColor, HorizontalMarkersWidth);

            }
            if (DrawSequenceTimelines)
            {
                var timelines = USequencerPlayer.Instances;
                var drawer = new USequencerInlineUIDrawer();

                foreach (var uSequencerPlayer in timelines)
                {
                    if(Selection.activeGameObject != uSequencerPlayer.gameObject)
                        drawer.Draw(uSequencerPlayer, 9.8f, false, false);
                }


            }
            Handles.color = oldColor;
            HandleUtility.Repaint();



        }

    }
}
