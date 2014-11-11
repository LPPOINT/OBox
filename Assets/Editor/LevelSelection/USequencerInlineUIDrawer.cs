using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameGUI.Pages.LevelSelection;
using Assets.Scripts.GameGUI.Pages.LevelSelection.USequencerIntegration;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.LevelSelection
{
    public class USequencerInlineUIDrawer
    {
        public USequencerInlineUIDrawer()
        {
            currentDragState = DragState.None;
        }

        private enum DragState
        {
            None,
            Drag
        }

        private enum DragSide
        {
            Start,
            End
        }

        private DragState currentDragState;
        private DragSide currentDragSide;

        private Vector2 lastMousePos ;

        public void Draw(USequencerPlayer s, float panelFallDownLenght = 9.8f, bool showEditHandles = true, bool drawInfo = true)
        {
            try
            {
                Handles.color = Color.white;
                var startPos = s.StartPosition.transform.position;
                var endPos = s.EndPosition.transform.position;

                if (showEditHandles)
                {
                    var newStartPosX = Handles.PositionHandle(startPos, Quaternion.identity).x;
                    startPos = new Vector3(newStartPosX, startPos.y, startPos.z);
                    s.StartPosition.transform.position = new Vector3(newStartPosX, startPos.y, startPos.z);

                    var newEndPosX = Handles.PositionHandle(endPos, Quaternion.identity).x;
                    endPos = new Vector3(newEndPosX, endPos.y, endPos.z);
                    s.EndPosition.transform.position = new Vector3(newEndPosX, endPos.y, endPos.z);
                }

                Handles.DrawLine(startPos, new Vector3(startPos.x, startPos.y - panelFallDownLenght, startPos.z));
                Handles.DrawLine(endPos, new Vector3(endPos.x, endPos.y - panelFallDownLenght, endPos.z));


                var h = 3;
                Handles.DrawSolidRectangleWithOutline(new Vector3[]
                                                      {
                                                          new Vector3(startPos.x, startPos.y - panelFallDownLenght, 0),
                                                          new Vector3(endPos.x, endPos.y - panelFallDownLenght, 0),
                                                          new Vector3(endPos.x, endPos.y - panelFallDownLenght - h, 0),
                                                          new Vector3(startPos.x, startPos.y - panelFallDownLenght - h,
                                                              0),
                                                      },
                    Color.white,
                    Color.black);

                Handles.color = Color.black;

                var currentMarkerX = s.CalculateLocalPosition(s.Sequencer.RunningTime);
                var markerWidth = 0.3f;

                Handles.color = Color.red;
                Handles.DrawSolidRectangleWithOutline(new Vector3[]
                                                      {
                                                          new Vector3(startPos.x + currentMarkerX,
                                                              startPos.y - panelFallDownLenght, 0),
                                                          new Vector3(startPos.x + currentMarkerX + markerWidth,
                                                              startPos.y - panelFallDownLenght, 0),
                                                          new Vector3(startPos.x + currentMarkerX + markerWidth,
                                                              startPos.y - panelFallDownLenght - h, 0),
                                                          new Vector3(startPos.x + currentMarkerX,
                                                              startPos.y - panelFallDownLenght - h, 0),
                                                      },
                    Color.red,
                    Color.white);

                if (drawInfo)
                {

                }
            }
            catch
            {
                
            }

        }


    }
}
