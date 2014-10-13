using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Camera.Effects
{
    public class CameraEffectOnEndLevel : LevelElement
    {

        private int endLevelEventLockId;

        protected override void OnLevelEnded()
        {

            endLevelEventLockId = LockCurrentEvent();
            
            base.OnLevelEnded();
        }
    }
}
